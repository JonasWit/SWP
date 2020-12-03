using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using SWP.DataBase;
using SWP.Domain.Enums;
using SWP.UI.Localization;
using SWP.UI.Services;
using System;

namespace SWP.UI
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        public Startup(IConfiguration configuration) => this.configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddHttpContextAccessor();

            services.AddDbContext<DataBase.AppContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DbConnection")),
                ServiceLifetime.Transient);

            services.AddDefaultIdentity<IdentityUser>(options => 
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 10;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.SignIn.RequireConfirmedAccount = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.AllowedForNewUsers = true;
            })
            .AddRoles<IdentityRole>()
            .AddErrorDescriber<PolishIdentityErrorDescriber>()
            .AddEntityFrameworkStores<DataBase.AppContext>();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "swp_u_data";
                options.ExpireTimeSpan = TimeSpan.FromHours(2);
            });

            //Email Confirmations
            services.AddMailKit(config =>
                config.UseMailKit(configuration.GetSection("Email").Get<MailKitOptions>()));    
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(ApplicationPolicy.LegalApplication.ToString(), policy => policy.RequireClaim(ClaimType.Application.ToString(), ApplicationType.LegalSwp.ToString()));
                options.AddPolicy("RootClient", policy => policy.RequireClaim(ClaimType.Status.ToString(), UserStatus.RootClient.ToString()));
            });

            services.AddServerSideBlazor();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
            });

            services.AddRazorPages();
            services.AddApplicationServices();
            services.AddDatabaseDeveloperPageExceptionFilter();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapRazorPages();
            });
        }
    }
}

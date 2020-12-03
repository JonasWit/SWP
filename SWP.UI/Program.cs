using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SWP.DataBase;
using SWP.Domain.Enums;
using System;
using System.Linq;
using System.Security.Claims;

namespace SWP.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.{env}.json")
                .Build();

            var host = CreateHostBuilder(args).Build();

            try
            {
                using var scope = host.Services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<DataBase.AppContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var rolesManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                context.Database.EnsureCreated();

                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();

                if (!context.Users.Any())
                {
                    rolesManager.CreateAsync(new IdentityRole(RoleType.Administrators.ToString())).GetAwaiter().GetResult();
                    rolesManager.CreateAsync(new IdentityRole(RoleType.Users.ToString())).GetAwaiter().GetResult();

                    var creatorUser = new IdentityUser()
                    {
                        UserName = "witviers@gmail.com",
                        Email = "witviers@gmail.com"
                    };

                    userManager.CreateAsync(creatorUser, "xyzAbc32167#").GetAwaiter().GetResult();
                    creatorUser.EmailConfirmed = true;

                    var creatorClaim = new Claim("Root", "Creator");
                    userManager.AddClaimAsync(creatorUser, creatorClaim).GetAwaiter().GetResult();
                    userManager.AddToRoleAsync(creatorUser, RoleType.Administrators.ToString()).GetAwaiter().GetResult();
                }

                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Exception in the Main.");
            }
            finally
            {
                Log.CloseAndFlush();   
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

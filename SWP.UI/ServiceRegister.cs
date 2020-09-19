using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using SWP.Application;
using SWP.DataBase.Managers;
using SWP.Domain.Infrastructure;
using SWP.Domain.Models.Licenses;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using SWP.UI.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SWP.UI
{
    public static class ServiceRegister
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection @this)
        {
            #region Add Services from the Application Project

            var transientServiceType = typeof(TransientService);
            var scopedServiceType = typeof(ScopedService);

            var appDefinedTypes = transientServiceType.Assembly.DefinedTypes;

            var transientServices = appDefinedTypes
                .Where(x => x.GetTypeInfo().GetCustomAttribute<TransientService>() != null);

            var scopedServices = appDefinedTypes
                .Where(x => x.GetTypeInfo().GetCustomAttribute<ScopedService>() != null);

            foreach (var service in transientServices)
            {
                @this.AddTransient(service);
            }

            foreach (var service in scopedServices)
            {
                @this.AddScoped(service);
            } 

            #endregion

            #region Add Services from the UI Project

            var uiTransientServiceType = typeof(UITransientService);
            var uiScopedServiceType = typeof(UIScopedService);

            var uiDefinedTypes = uiTransientServiceType.Assembly.DefinedTypes;

            var uiTransientServices = uiDefinedTypes
                .Where(x => x.GetTypeInfo().GetCustomAttribute<UITransientService>() != null);

            var uiScopedServices = uiDefinedTypes
                .Where(x => x.GetTypeInfo().GetCustomAttribute<UIScopedService>() != null);

            foreach (var service in uiTransientServices)
            {
                @this.AddTransient(service);
            }

            foreach (var service in uiScopedServices)
            {
                @this.AddScoped(service);
            } 

            #endregion

            @this.AddTransient<ILegalSwpManager, LegalSwpManager>();
            @this.AddTransient<IMedicalSwpManager, MedicalSwpManager>();
            @this.AddTransient<ILicensesManager, LicensesManager>();

            @this.AddScoped<DialogService>();
            @this.AddScoped<NotificationService>();

            //@this.AddTransient<CasesPanel>();
            //@this.AddTransient<CalendarPanel>();
            //@this.AddTransient<CustomersPanel>();
            //@this.AddTransient<LegalSwpApp>();


            //@this.AddTransient<IProductManager, ProductManager>();
            //@this.AddTransient<IOrderManager, OrderManager>();
            //@this.AddTransient<IFileManager, FileManager>();
            //@this.AddTransient<INewsManager, NewsManager>();
            //@this.AddTransient<ILogManager, LogManager>();
            //@this.AddTransient<IShopManager, ShopManager>();
            //@this.AddTransient<ICrawlersDataBaseManager, CrawlersDataBaseManager>();

            //@this.AddScoped<ISessionManager, SessionManager>();

            //@this.AddSingleton<AppSettingsService>();

            //// Add Quartz services
            //@this.AddSingleton<IJobFactory, SingletonJobFactory>();
            //@this.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            //@this.AddSingleton<JobWakeUpCall>();
            //@this.AddSingleton(new JobSchedule(
            //    jobType: typeof(JobWakeUpCall),
            //    cronExpression: "0 0/10 * * * ?"));

            //@this.AddSingleton<JobRunCrawlers>();
            //@this.AddSingleton(new JobSchedule(
            //    jobType: typeof(JobRunCrawlers),
            //    cronExpression: "0 15 4 ? * *"));

            //@this.AddHostedService<QuartzHostedService>();

            return @this;
        }
    }
}

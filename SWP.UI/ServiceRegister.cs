using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Radzen;
using SWP.Application;
using SWP.DataBase.Managers;
using SWP.Domain.Infrastructure;
using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Infrastructure.Portal;
using SWP.UI.Automation;
using SWP.UI.BlazorApp;
using System.Linq;
using System.Reflection;

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

            @this.AddTransient<ILegalManager, LegalManager>();
            @this.AddTransient<IStatisticsManager, StatisticsManager>();
            @this.AddTransient<ILogManager, LogManager>();
            @this.AddTransient<IFileManager, FileManager>();
            @this.AddTransient<INewsManager, NewsManager>();

            @this.AddScoped<DialogService>();
            @this.AddScoped<TooltipService>();
            @this.AddScoped<NotificationService>();

            @this.AddScoped<IActionDispatcher, ActionDispatcher>();

            //@this.AddScoped<ISessionManager, SessionManager>();

            //// Add Quartz services
            @this.AddSingleton<IJobFactory, SingletonJobFactory>();
            @this.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            @this.AddSingleton<JobWakeUpCall>();
            @this.AddSingleton(new JobSchedule(
                jobType: typeof(JobWakeUpCall),
                cronExpression: "0 0/10 * * * ?"));

            @this.AddHostedService<QuartzHostedService>();

            return @this;
        }
    }
}

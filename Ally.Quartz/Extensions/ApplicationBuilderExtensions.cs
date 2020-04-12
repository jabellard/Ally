using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace Ally.Quartz.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseQuartz(this IApplicationBuilder builder)
        {
            var serviceProvider = builder.ApplicationServices;
            var scheduler = serviceProvider.GetRequiredService<IScheduler>();
            scheduler.JobFactory = serviceProvider.GetRequiredService<IJobFactory>();
            
            var schedulerListeners = serviceProvider.GetRequiredService<IEnumerable<ISchedulerListener>>();
            foreach (var schedulerListener in schedulerListeners)
                scheduler.ListenerManager.AddSchedulerListener(schedulerListener);
            
            var triggerListeners = serviceProvider.GetRequiredService<IEnumerable<ITriggerListener>>();
            foreach (var triggerListener in triggerListeners)
                scheduler.ListenerManager.AddTriggerListener(triggerListener);
            
            var jobListeners = serviceProvider.GetRequiredService<IEnumerable<IJobListener>>();
            foreach (var jobListener in jobListeners)
                scheduler.ListenerManager.AddJobListener(jobListener);

            scheduler.Start().Wait();
            return builder;
        }
    }
}
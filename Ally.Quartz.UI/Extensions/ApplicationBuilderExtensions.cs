using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartzmin;

namespace Ally.Quartz.UI.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseQuartzUi(this IApplicationBuilder builder)
        {
            builder.UseQuartzmin(new QuartzminOptions
            {
                Scheduler = builder.ApplicationServices.GetRequiredService<IScheduler>()
            });
            return builder;
        }
    }
}
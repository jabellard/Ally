using System;
using Microsoft.Extensions.DependencyInjection;

namespace Ally.Quartz.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddQuartz(this IServiceCollection services,
            Action<QuartzConfigurator> configuration)
        {
            var configurator = new QuartzConfigurator(services);
            configuration(configurator);
            configurator.Configure();
            return services;
        }
    }
}
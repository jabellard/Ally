using System;
using Microsoft.Extensions.DependencyInjection;

namespace Ally.Diagnostics.HealthChecks.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHealthChecks(this IServiceCollection services,
            Action<HealthCheckConfigurator> configuration)
        {
            var healthChecksBuilder = services.AddHealthChecks();
            var configurator = new HealthCheckConfigurator(healthChecksBuilder);
            configuration(configurator);
            configurator.Configure();
            return services;
        }
    }
}
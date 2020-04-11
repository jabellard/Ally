using System;
using Microsoft.Extensions.DependencyInjection;

namespace Ally.Logging.Http.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRequestLogging(this IServiceCollection services, Action<RequestLoggingConfigurator> configuration)
        {
            var configurator = new RequestLoggingConfigurator(services);
            configuration(configurator);
            configurator.Configure();
            return services;
        }
    }
}
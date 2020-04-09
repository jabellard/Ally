using System;
using Microsoft.Extensions.DependencyInjection;

namespace Ally.RabbitMQ.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services, Action<RabbitMqConfigurator> configuration)
        {
            var configurator = new RabbitMqConfigurator(services);
            configuration(configurator);
            configurator.Configure();
            return services;
        }
    }
}
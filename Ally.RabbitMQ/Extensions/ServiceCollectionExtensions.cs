using Microsoft.Extensions.DependencyInjection;

namespace Ally.RabbitMQ.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services)
        {
            return services;
        }
    }
}
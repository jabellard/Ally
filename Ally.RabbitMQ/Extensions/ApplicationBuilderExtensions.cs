using Ally.RabbitMQ.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ally.RabbitMQ.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseRabbitMq(this IApplicationBuilder builder)
        {
            var applicationLifetime = builder
                .ApplicationServices.
                GetService<IHostApplicationLifetime>();
            
            applicationLifetime.ApplicationStopping.Register(() =>
            {
                var rabbitMqConnections = builder
                    .ApplicationServices.
                    GetServices<IRabbitMqConnection>();
                foreach (var rabbitMqConnection in rabbitMqConnections)
                {
                    rabbitMqConnection
                        .Connection
                        .Close();
                }
            });
            return builder;
        }
    }
}
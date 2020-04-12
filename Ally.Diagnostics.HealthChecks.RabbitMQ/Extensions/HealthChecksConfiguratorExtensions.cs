using System.Collections.Generic;
using Ally.RabbitMQ.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Ally.Diagnostics.HealthChecks.RabbitMQ.Extensions
{
    public static class HealthChecksConfiguratorExtensions
    {
        public static HealthCheckConfigurator AddRabbitMq(this HealthCheckConfigurator configurator,
            string name = "RabbitMQ", HealthStatus? failureStatus = default, IEnumerable<string> tags = default)
        {
            return configurator.Add(new HealthCheckRegistration(name,
                sp => new RabbitMqHealthCheck(sp.GetRequiredService<IRabbitMqConnection>()),
                failureStatus,
                tags));
        }
    }
}
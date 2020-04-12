using System;
using System.Threading;
using System.Threading.Tasks;
using Ally.RabbitMQ.Abstractions;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Ally.Diagnostics.HealthChecks.RabbitMQ
{
    public class RabbitMqHealthCheck: IHealthCheck
    {
        private readonly IRabbitMqConnection _rabbitMqConnection;

        public RabbitMqHealthCheck(IRabbitMqConnection rabbitMqConnection)
        {
            _rabbitMqConnection = rabbitMqConnection;
        }
        
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                using var channel = _rabbitMqConnection
                    .Connection
                    .CreateModel();
                return Task.FromResult(HealthCheckResult.Healthy());
            }
            catch (Exception e)
            {
                return Task.FromResult(new HealthCheckResult(context.Registration.FailureStatus, exception: e));
            }
        }
    }
}
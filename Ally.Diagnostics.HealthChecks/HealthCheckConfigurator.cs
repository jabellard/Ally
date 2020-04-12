using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Ally.Diagnostics.HealthChecks
{
    public class HealthCheckConfigurator
    {
        public IHealthChecksBuilder Builder { get; }
        public IServiceCollection Services => Builder.Services;

        internal HealthCheckConfigurator(IHealthChecksBuilder healthChecksBuilder)
        {
            Builder = healthChecksBuilder;
        }

        public HealthCheckConfigurator Add(HealthCheckRegistration registration)
        {
            Builder.Add(registration);
            return this;
        }
        
        internal void Configure(){}
    }
}
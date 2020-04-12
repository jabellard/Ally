using System;
using HealthChecks.UI.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ally.Diagnostics.HealthChecks.UI.Extensions
{
    public static class HealthChecksConfiguratorExtensions
    {
        public static HealthCheckConfigurator AddUi(this HealthCheckConfigurator configurator,
            string databaseName = "healthChecksDb", Action<Settings> configuration = default)
        {
            configurator.Services.AddHealthChecksUI(databaseName, configuration);
            return configurator;
        }
    }
}
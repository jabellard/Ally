using System;
using Ally.Hosting.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ally.Hosting.Extensions
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder ConfigureApplicationHost(this IHostBuilder hostBuilder,
            Action<IApplicationHostBuilder> configuration)
        {
            var applicationHostBuilder = new ApplicationHostBuilder(hostBuilder);
            configuration(applicationHostBuilder);
            hostBuilder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<IHostedService, ApplicationExecutor>();
            });
            return hostBuilder;
        }
    }
}
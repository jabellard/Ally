using System;
using Ally.Hosting.Abstractions;
using Ally.Hosting.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ally.Hosting
{
    public class ApplicationHostBuilder: IApplicationHostBuilder
    {
        private readonly IHostBuilder _hostBuilder;

        public ApplicationHostBuilder(IHostBuilder hostBuilder)
        {
            _hostBuilder = hostBuilder;
            _hostBuilder.ConfigureServices((context, services) =>
                {
                    services.AddSingleton<IApplicationConfiguratorFactory, ApplicationConfiguratorFactory>();
                });
        }

        public IApplicationHost Build()
        {
            throw new NotSupportedException($"Building of {nameof(IApplicationHostBuilder)} is not supported.");
        }

        public IApplicationHostBuilder WithConfiguration(Action<IConfigurationBuilder> configuration)
        {
            _hostBuilder.ConfigureAppConfiguration((context, configurationBuilder) =>
            {
                configuration(configurationBuilder);
            });
            return this;
        }

        public IApplicationHostBuilder WithServices(Action<IServiceCollection> configuration)
        {
            _hostBuilder.ConfigureServices((context, services) => { configuration(services); });
            return this;
        }

        public IApplicationHostBuilder WithApplicationConfiguration(Action<IApplicationConfigurator> configuration)
        {
            _hostBuilder.ConfigureServices((context, services) =>
                {
                    services.Configure<ApplicationExecutorOptions>(options =>
                        {
                            options.ApplicationConfiguration = configuration;
                        });
                });
            return this;
        }

        public IApplicationHostBuilder WithStartUp<TApplicationStartup>() where TApplicationStartup : IApplicationStartup
        {
            _hostBuilder.ConfigureServices((context, services) =>
            {
                services.Configure<ApplicationExecutorOptions>(options =>
                {
                    var serviceProvider = services.BuildServiceProvider();
                    var applicationStartup = ActivatorUtilities.CreateInstance<TApplicationStartup>(serviceProvider);
                    applicationStartup.ConfigureServices(services);
                    services.Configure<ApplicationExecutorOptions>(options =>
                    {
                        options.ApplicationConfiguration = applicationStartup.ConfigureApplication;
                    });
                });
            });
            return this;
        }

        public IApplicationHostBuilder WithApplication<TApplication>() where TApplication : IApplication
        {
            _hostBuilder.ConfigureServices((context, services) =>
            {
                services.Add(new ServiceDescriptor(typeof(IApplication), typeof(TApplication)));
            });
            return this;
        }
    }
}
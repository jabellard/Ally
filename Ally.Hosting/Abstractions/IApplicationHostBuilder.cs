using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ally.Hosting.Abstractions
{
    public interface IApplicationHostBuilder
    {
        IApplicationHost Build();
        IApplicationHostBuilder WithConfiguration(Action<IConfigurationBuilder> configuration);
        IApplicationHostBuilder WithServices(Action<IServiceCollection> configuration);
        IApplicationHostBuilder WithApplicationConfiguration(Action<IApplicationConfigurator> configuration);
        IApplicationHostBuilder WithStartUp<TApplicationStartup>() where TApplicationStartup : IApplicationStartup;
        IApplicationHostBuilder WithApplication<TApplication>() where TApplication : IApplication;
    }
}
using System;
using Ally.Logging.Http.Abstractions;
using Ally.Logging.Http.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IO;

namespace Ally.Logging.Http
{
    public class RequestLoggingConfigurator
    {
        public IServiceCollection Services { get; }

        internal RequestLoggingConfigurator(IServiceCollection services)
        {
            Services = services;
        }
        
        public RequestLoggingConfigurator WithOptions(IConfigurationSection configuration)
        {
            Services.Configure<RequestLoggingOptions>(configuration);
            return this;
        }

        public RequestLoggingConfigurator WithOptions(Action<RequestLoggingOptions> configuration)
        {
            Services.Configure(configuration);
            return this;
        }
        
        public RequestLoggingConfigurator WithSingletonLogger<TLogger>()
            where TLogger: IRequestLogger
        {
            return WithLogger<TLogger>(ServiceLifetime.Singleton);
        }
        
        public RequestLoggingConfigurator WithScopedLogger<TLogger>()
            where TLogger: IRequestLogger
        {
            return WithLogger<TLogger>(ServiceLifetime.Scoped);
        }
        
        public RequestLoggingConfigurator WithTransientLogger<TLogger>()
            where TLogger: IRequestLogger
        {
            return WithLogger<TLogger>(ServiceLifetime.Transient);
        }

        public RequestLoggingConfigurator WithLogger<TLogger>(ServiceLifetime serviceLifetime)
            where TLogger: IRequestLogger
        {
            return WithLogger(typeof(TLogger), serviceLifetime);
        }

        public RequestLoggingConfigurator WithLogger(Type requestLogger, ServiceLifetime serviceLifetime)
        {
            Services.Add(new ServiceDescriptor(typeof(IRequestLogger), requestLogger, serviceLifetime));
            return this;
        }

        internal void Configure()
        {
            Services.AddOptions();
            Services.Configure<RequestLoggingOptions>(o => { });
            Services.TryAddSingleton(f => new RecyclableMemoryStreamManager());
        }
    }
}
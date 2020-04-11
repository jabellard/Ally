using System;
using System.Collections.Generic;
using System.Linq;
using Ally.RabbitMQ.Abstractions;
using Ally.RabbitMQ.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Ally.RabbitMQ
{
    public class RabbitMqConfigurator
    {
        public IServiceCollection Services { get; }
        private readonly List<RabbitMqConnectionOptions> _connectionOptionsCollection;
        private readonly List<Tuple<Action<ConnectionFactory>, Func<RabbitMqConnectionOptions, bool>>>
            _connectionOptionsConfigurations;

        internal RabbitMqConfigurator(IServiceCollection services)
        {
            Services = services;
            _connectionOptionsCollection = new List<RabbitMqConnectionOptions>();
            _connectionOptionsConfigurations = new List<Tuple<Action<ConnectionFactory>, Func<RabbitMqConnectionOptions, bool>>>();
        }

        public RabbitMqConfigurator FromConfiguration(IConfigurationSection configuration)
        {
            var connectionConfigurations = configuration
                .GetSection($"{nameof(RabbitMqOptions.Connections)}")
                .GetChildren()
                .ToList();

            foreach (var connectionConfiguration in connectionConfigurations)
            {
                var connectionOptions = new RabbitMqConnectionOptions();
                connectionConfiguration.Bind(connectionOptions);
                _connectionOptionsCollection.Add(connectionOptions);
            }

            var publisherConfigurations = configuration
                .GetSection($"{nameof(RabbitMqOptions.Publishers)}")
                .GetChildren()
                .ToList();

            foreach (var publisherConfiguration in publisherConfigurations)
                Services
                    .Configure<RabbitMqPublisherOptions>(publisherConfiguration[$"{nameof(RabbitMqPublisherOptions.Name)}"], publisherConfiguration);
            
            var consumerConfigurations = configuration
                .GetSection($"{nameof(RabbitMqOptions.Consumers)}")
                .GetChildren()
                .ToList();
            
            foreach (var consumerConfiguration in consumerConfigurations)
                Services
                    .Configure<RabbitMqConsumerOptions>(consumerConfiguration[$"{nameof(RabbitMqConsumerOptions.Name)}"], consumerConfiguration);

            return this;
        }

        public RabbitMqConfigurator WithConnection(Action<ConnectionFactory> connectionConfigurator, Func<RabbitMqConnectionOptions, bool> connectionSelector)
        {
            _connectionOptionsConfigurations.Add(new Tuple<Action<ConnectionFactory>, Func<RabbitMqConnectionOptions, bool>>(connectionConfigurator, connectionSelector));
            return this;
        }

        public RabbitMqConfigurator WithSingletonPublisher<TPublisher>()
            where TPublisher: IRabbitMqPublisher
        {
            return WithPublisher<TPublisher>(ServiceLifetime.Singleton);
        }
        
        public RabbitMqConfigurator WithScopedPublisher<TPublisher>()
            where TPublisher: IRabbitMqPublisher
        {
            return WithPublisher<TPublisher>(ServiceLifetime.Scoped);
        }
        
        public RabbitMqConfigurator WithTransientPublisher<TPublisher>()
            where TPublisher: IRabbitMqPublisher
        {
            return WithPublisher<TPublisher>(ServiceLifetime.Transient);
        }
        
        public RabbitMqConfigurator WithPublisher<TPublisher>(ServiceLifetime serviceLifetime)
            where TPublisher: IRabbitMqPublisher
        {
            return WithPublisher(typeof(TPublisher), serviceLifetime);
        }
        

        public RabbitMqConfigurator WithSingletonConsumer<TConsumer>()
            where TConsumer: IRabbitMqConsumer
        {
            return WithConsumer<TConsumer>(ServiceLifetime.Singleton);
        }
        
        public RabbitMqConfigurator WithScopedConsumer<TConsumer>()
            where TConsumer: IRabbitMqConsumer
        {
            return WithConsumer<TConsumer>(ServiceLifetime.Scoped);
        }
        
        public RabbitMqConfigurator WithTransientConsumer<TConsumer>()
            where TConsumer: IRabbitMqConsumer
        {
            return WithConsumer<TConsumer>(ServiceLifetime.Transient);
        }
        
        public RabbitMqConfigurator WithConsumer<TConsumer>(ServiceLifetime serviceLifetime)
            where TConsumer: IRabbitMqConsumer
        {
            return WithConsumer(typeof(TConsumer), serviceLifetime);
        }
        
        public RabbitMqConfigurator WithPublisher(Type publisherType, ServiceLifetime serviceLifetime)
        {
            Services.Add(new ServiceDescriptor(typeof(IRabbitMqPublisher), publisherType, serviceLifetime));
            return this;
        }
        
        public RabbitMqConfigurator WithConsumer(Type consumerType, ServiceLifetime serviceLifetime)
        {
            Services.Add(new ServiceDescriptor(typeof(IRabbitMqConsumer), consumerType, serviceLifetime));
            return this;
        }



        internal void Configure()
        {
            Services.AddOptions();
            foreach (var connectionOptions in _connectionOptionsCollection)
            {
                foreach (var (connectionConfigurator, connectionSelector) in _connectionOptionsConfigurations)
                    if (connectionSelector(connectionOptions))
                        connectionConfigurator(connectionOptions.ConnectionFactory);
                
                var rabbitMqConnection = new RabbitMqConnection
                {
                    Name = connectionOptions.Name,
                    Connection = connectionOptions.ConnectionFactory.CreateConnection()
                };
                Services.AddSingleton<IRabbitMqConnection>(f => rabbitMqConnection);
            }
        }
    }
}
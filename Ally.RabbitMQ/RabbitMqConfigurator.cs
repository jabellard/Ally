using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Ally.RabbitMQ.Abstractions;
using Ally.RabbitMQ.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Ally.RabbitMQ
{
    public class RabbitMqConfigurator
    {
        public IServiceCollection Services { get; private set; }
        private List<RabbitMqConnectionOptions> ConnectionOptionsCollection { get; set; } = new List<RabbitMqConnectionOptions>();
        private List<Tuple<Action<ConnectionFactory>, Func<RabbitMqConnectionOptions, bool>>> ConnectionOptionsConfigurations = new List<Tuple<Action<ConnectionFactory>, Func<RabbitMqConnectionOptions, bool>>>();

        public RabbitMqConfigurator(IServiceCollection services)
        {
            Services = services;
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
                ConnectionOptionsCollection.Add(connectionOptions);
            }

            var publisherConfigurations = configuration
                .GetSection($"{nameof(RabbitMqOptions.Publishers)}")
                .GetChildren()
                .ToList();

            foreach (var publisherConfiguration in publisherConfigurations)
            {
                Services
                    .Configure<RabbitMqPublisherOptions>(publisherConfiguration[$"{nameof(RabbitMqPublisherOptions.Name)}"], publisherConfiguration);
            }
            
            var consumerConfigurations = configuration
                .GetSection($"{nameof(RabbitMqOptions.Consumers)}")
                .GetChildren()
                .ToList();
            
            foreach (var consumerConfiguration in consumerConfigurations)
            {
                Services
                    .Configure<RabbitMqConsumerOptions>(consumerConfiguration[$"{nameof(RabbitMqConsumerOptions.Name)}"], consumerConfiguration);
            }

            return this;
        }

        public RabbitMqConfigurator WithConnection(Action<ConnectionFactory> connectionConfigurator, Func<RabbitMqConnectionOptions, bool> connectionSelector)
        {
            ConnectionOptionsConfigurations.Add(new Tuple<Action<ConnectionFactory>, Func<RabbitMqConnectionOptions, bool>>(connectionConfigurator, connectionSelector));
            return this;
        }

        public RabbitMqConfigurator WithPublisher<TPublisher>(ServiceLifetime serviceLifetime)
            where TPublisher: IRabbitMqConsumer
        {
            return WithPublisher(typeof(TPublisher), serviceLifetime);
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



        public void Configure()
        {
            // TODO: Takes O(N^2). Optimize.
            foreach (var connectionOptions in ConnectionOptionsCollection)
            {
                foreach (var (connectionConfigurator, connectionSelector) in ConnectionOptionsConfigurations)
                {
                    if (connectionSelector(connectionOptions))
                    {
                        connectionConfigurator(connectionOptions.ConnectionFactory);
                    }
                    
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
}
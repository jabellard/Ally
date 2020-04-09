using Ally.RabbitMQ.Abstractions;
using RabbitMQ.Client;

namespace Ally.RabbitMQ
{
    public class RabbitMqConnection: IRabbitMqConnection
    {
        public string Name { get; set; }
        public IConnection Connection { get; set; }
    }
}
using RabbitMQ.Client;

namespace Ally.RabbitMQ.Abstractions
{
    public interface IRabbitMqConnection
    {
        string Name { get; set; }
        IConnection Connection { get; set; }
    }
}
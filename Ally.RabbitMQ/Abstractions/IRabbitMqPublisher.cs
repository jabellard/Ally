using System.Threading.Tasks;

namespace Ally.RabbitMQ.Abstractions
{
    public interface IRabbitMqPublisher
    {
        Task Publish(object message, string routingKey);
    }
}
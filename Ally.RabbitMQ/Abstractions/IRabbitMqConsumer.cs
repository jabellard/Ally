using System.Threading.Tasks;

namespace Ally.RabbitMQ.Abstractions
{
    public interface IRabbitMqConsumer
    {
        Task Start();
    }
}
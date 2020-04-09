using System.Collections.Generic;

namespace Ally.RabbitMQ.Options
{
    public class RabbitMqOptions
    {
        public List<RabbitMqConnectionOptions> Connections { get; set; }
        public List<RabbitMqPublisherOptions> Publishers { get; set; }
        public List<RabbitMqConsumerOptions> Consumers { get; set; }
    }
}
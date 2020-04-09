using System.Collections.Generic;
using RabbitMQ.Client;

namespace Ally.RabbitMQ.Options
{
    public class RabbitMqConnectionOptions
    {
        public string Name { get; set; }
        public ConnectionFactory ConnectionFactory { get; set; }
    }
}
using System.Collections.Generic;

namespace Ally.RabbitMQ.Options
{
    public class RabbitMqConsumerOptions
    {
        public string Name { get; set; }
        public string Queue { get; set; }
        public string Exchange { get; set; }
        public List<string> BindingKeys { get; set; }
        public Dictionary<string, object> Data { get; set; }
    }
}
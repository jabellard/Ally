using System.Collections.Generic;

namespace Ally.Http.Options
{
    public class HttpClientOptions
    {
        public string BaseAddress { get; set; }
        public Dictionary<string, string> DefaultRequestHeaders { get; set; }
        public long? MaxResponseContentBufferSize { get; set; }
        public int? TimeOutInSeconds { get; set; }
    }
}
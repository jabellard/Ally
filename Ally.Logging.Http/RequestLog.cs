using Ally.Logging.Http.Abstractions;

namespace Ally.Logging.Http
{
    public class RequestLog: IRequestLog
    {
        public string User { get; set; }
        public string Method { get; set; }
        public string Path { get; set; }
        public string QueryString { get; set; }
        public string RequestBody { get; set; }
        public int StatusCode { get; set; }
        public string ResponseBody { get; set; }
    }
}
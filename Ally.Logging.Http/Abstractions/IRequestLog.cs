namespace Ally.Logging.Http.Abstractions
{
    public interface IRequestLog
    {
        string User { get; set; }
        string Method { get; set; }
        string Path { get; set; }
        string QueryString { get; set; }
        string RequestBody { get; set; }
        int StatusCode { get; set; }
        string ResponseBody { get; set; }
    }
}
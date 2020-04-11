using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Ally.Logging.Http.Abstractions;
using Ally.Logging.Http.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IO;

namespace Ally.Logging.Http
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        private readonly RequestLoggingOptions _requestLoggingOptions;
        private readonly IEnumerable<IRequestLogger> _requestLoggers;

        public RequestLoggingMiddleware(RequestDelegate next, RecyclableMemoryStreamManager recyclableMemoryStreamManager,
            IOptions<RequestLoggingOptions> optionsAccessor, IEnumerable<IRequestLogger> requestLoggers)
        {
            _next = next;
            _recyclableMemoryStreamManager = recyclableMemoryStreamManager;
            _requestLoggingOptions = optionsAccessor.Value;
            _requestLoggers = requestLoggers;
        }

        public async Task Invoke(HttpContext context)
        {
            if (_requestLoggingOptions.Disabled)
            {
                await _next(context);
                return;
            }
            
            var requestBody = await GetRequestBody(context.Request);
            var responseBody = await GetResponseBody(context);
            var requestLog = new RequestLog
            {
                User = context.User.Identity.Name,
                Method = context.Request.Method,
                Path = context.Request.Path,
                QueryString = context.Request.QueryString.ToString(),
                RequestBody = requestBody,
                StatusCode = context.Response.StatusCode,
                ResponseBody = responseBody
            };

            foreach (var requestLogger in _requestLoggers)
                await requestLogger.Log(requestLog);
        }

        private async Task<string> GetRequestBody(HttpRequest request)
        {
            request.EnableBuffering();
            await using var requestBodyStream = _recyclableMemoryStreamManager.GetStream();
            await request.Body.CopyToAsync(requestBodyStream);
            requestBodyStream.Seek(0, SeekOrigin.Begin);
            using var streamReader = new StreamReader(requestBodyStream);
            var requestBodyText = await streamReader.ReadToEndAsync();
            request.Body.Seek(0, SeekOrigin.Begin);
            return requestBodyText;
        }

        private async Task<string> GetResponseBody(HttpContext context)
        {
            var originalResponseBodyStream = context.Response.Body;
            await using var responseBodyStream = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseBodyStream;
            await _next(context);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            using var streamReader = new StreamReader(context.Response.Body);
            var responseBodyText = await streamReader.ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(originalResponseBodyStream);
            return responseBodyText;
        }
    }
}
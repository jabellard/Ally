using System.Threading.Tasks;
using Ally.Logging.Http.Abstractions;

namespace Ally.Logging.Http.RequestLoggers
{
    public class RequestLogger: IRequestLogger
    {
        public Task Log(IRequestLog requestLog)
        {
            return Task.CompletedTask;
        }
    }
}
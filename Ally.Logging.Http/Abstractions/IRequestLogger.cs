using System.Threading.Tasks;

namespace Ally.Logging.Http.Abstractions
{
    public interface IRequestLogger
    {
        Task Log(IRequestLog requestLog);
    }
}
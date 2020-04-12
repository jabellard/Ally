using System.Threading;
using System.Threading.Tasks;

namespace Ally.Hosting.Abstractions
{
    public interface IApplication
    {
        Task Start(CancellationToken cancellationToken);
        Task Stop(CancellationToken cancellationToken);
    }
}
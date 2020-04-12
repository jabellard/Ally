using Microsoft.Extensions.DependencyInjection;

namespace Ally.Hosting.Abstractions
{
    public interface IServiceConfigurator
    {
        void Configure(IServiceCollection services);
    }
}
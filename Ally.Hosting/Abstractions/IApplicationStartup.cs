using Microsoft.Extensions.DependencyInjection;

namespace Ally.Hosting.Abstractions
{
    public interface IApplicationStartup
    {
        void ConfigureServices(IServiceCollection services);
        void ConfigureApplication(IApplicationConfigurator configurator);
    }
}
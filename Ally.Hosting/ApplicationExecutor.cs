using System.Threading;
using System.Threading.Tasks;
using Ally.Hosting.Abstractions;
using Ally.Hosting.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Ally.Hosting
{
    public class ApplicationExecutor: IHostedService
    {
        private readonly IApplication _application;
        private readonly IApplicationConfiguratorFactory _applicationConfiguratorFactory;
        private readonly ApplicationExecutorOptions _applicationExecutorOptions;
        public ApplicationExecutor(IOptions<ApplicationExecutorOptions> optionsAccessor,
            IApplication application,
            IApplicationConfiguratorFactory applicationConfiguratorFactory)
        {
            _application = application;
            _applicationConfiguratorFactory = applicationConfiguratorFactory;
            _applicationExecutorOptions = optionsAccessor.Value;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (_applicationExecutorOptions.ApplicationConfiguration != null)
            {
                var applicationConfigurator = _applicationConfiguratorFactory.Create();
                _applicationExecutorOptions.ApplicationConfiguration(applicationConfigurator);
            }
            await _application.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _application.Stop(cancellationToken);
        }
    }
}
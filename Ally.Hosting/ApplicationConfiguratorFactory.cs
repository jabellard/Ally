using System;
using Ally.Hosting.Abstractions;

namespace Ally.Hosting
{
    public class ApplicationConfiguratorFactory: IApplicationConfiguratorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ApplicationConfiguratorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public IApplicationConfigurator Create()
        {
            return new ApplicationConfigurator(_serviceProvider);
        }
    }
}
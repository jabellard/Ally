using System;
using Ally.Hosting.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Ally.Testing.XUnit
{
    public class IoCFixture<TTerminalServiceConfigurator> where  TTerminalServiceConfigurator: ITerminalServiceConfigurator
    {
        public IServiceProvider ServiceProvider { get; }
        
        public IoCFixture()
        {
            var terminalServiceConfigurator = Activator.CreateInstance<ITerminalServiceConfigurator>();
            var services = new ServiceCollection();
            ServiceProvider = terminalServiceConfigurator.Configure(services);
        }
    }
}
using System;
using Microsoft.Extensions.DependencyInjection;

namespace Ally.Hosting.Abstractions
{
    public interface ITerminalServiceConfigurator
    {
        IServiceProvider Configure(IServiceCollection services);
    }
}
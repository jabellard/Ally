using System;
using Ally.Hosting.Abstractions;

namespace Ally.Hosting.Options
{
    public class ApplicationExecutorOptions
    {
        public Action<IApplicationConfigurator> ApplicationConfiguration { get; set; }
    }
}
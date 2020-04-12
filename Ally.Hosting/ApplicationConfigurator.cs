using System;
using Ally.Hosting.Abstractions;

namespace Ally.Hosting
{
    public class ApplicationConfigurator: IApplicationConfigurator
    {
        public IServiceProvider ApplicationServices { get; }

        public ApplicationConfigurator(IServiceProvider serviceProvider)
        {
            ApplicationServices = serviceProvider;
        }
    }
}
using System;
using System.ComponentModel.Design;

namespace Ally.Hosting.Abstractions
{
    public interface IApplicationConfigurator
    {
       IServiceProvider ApplicationServices { get; }
    }
}
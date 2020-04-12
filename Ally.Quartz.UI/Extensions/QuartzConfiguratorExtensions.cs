using Quartzmin;

namespace Ally.Quartz.UI.Extensions
{
    public static class QuartzConfiguratorExtensions
    {
        public static QuartzConfigurator AddQuartzUi(this QuartzConfigurator configurator)
        {
            configurator.Services.AddQuartzmin();
            return configurator;
        }
    }
}
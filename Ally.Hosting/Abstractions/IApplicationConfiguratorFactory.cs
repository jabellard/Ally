namespace Ally.Hosting.Abstractions
{
    public interface IApplicationConfiguratorFactory
    {
        IApplicationConfigurator Create();
    }
}
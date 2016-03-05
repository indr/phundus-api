namespace Phundus.Common
{
    using System.Reflection;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Commanding;
    using Eventing;

    public class CommonInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IEventSerializer>().ImplementedBy<EventSerializer>());

            new CommandHandlerInstaller().Install(container, Assembly.GetExecutingAssembly());
        }
    }
}
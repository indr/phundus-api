namespace Phundus
{
    using System;
    using System.Reflection;
    using System.Web.Http.Controllers;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Common.Commanding;
    using Common.Eventing.Installers;
    using Common.Projecting.Installers;
    using Common.Resources;

    public class AssemblyInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var assembly = Assembly.GetExecutingAssembly();
            new CommandHandlerInstaller().Install(container, assembly);
            new ProjectionsInstaller().Install(container, assembly);
            new EventHandlerInstaller().Install(container, assembly);

            
        }
    }
}
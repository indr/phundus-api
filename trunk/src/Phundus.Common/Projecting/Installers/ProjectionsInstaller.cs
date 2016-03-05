namespace Phundus.Common.Projecting.Installers
{
    using System.Reflection;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Injecting;

    public class ProjectionsInstaller : AssemblyInstallerBase
    {
        public override void Install(IWindsorContainer container, Assembly assembly)
        {
            container.Register(Classes.FromAssembly(assembly)
                .BasedOn<ProjectionBase>()
                .WithServiceAllInterfaces());
        }
    }
}
namespace Phundus.Common.Projecting
{
    using System;
    using System.Reflection;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Injecting;

    public class ProjectionsInstaller : IAssemblyInstaller
    {
        public void Install(IWindsorContainer container, Assembly assembly)
        {
            container.Register(Types.FromAssembly(assembly)
                .BasedOn<ProjectionBase>()
                .WithServiceAllInterfaces());
        }
    }

    public interface IProjection
    {
        void Reset();        
        Type GetEntityType();
    }
}
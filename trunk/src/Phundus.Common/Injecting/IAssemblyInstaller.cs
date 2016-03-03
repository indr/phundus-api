namespace Phundus.Common.Injecting
{
    using System;
    using System.Reflection;
    using Castle.Windsor;

    public interface IAssemblyInstaller
    {
        void Install(IWindsorContainer container);
        void Install(IWindsorContainer container, Assembly assembly);
    }

    public abstract class AssemblyInstallerBase : IAssemblyInstaller
    {
        public void Install(IWindsorContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");
            Install(container, Assembly.GetExecutingAssembly());
        }

        public abstract void Install(IWindsorContainer container, Assembly assembly);
    }
}
namespace Phundus.Common.Injecting
{
    using System;
    using System.Reflection;
    using Castle.Windsor;

    public interface IAssemblyInstaller
    {
        void Install(IWindsorContainer container, Type assemblyOfType);
        void Install(IWindsorContainer container, Assembly assembly);
    }

    public abstract class AssemblyInstallerBase : IAssemblyInstaller
    {
        public void Install(IWindsorContainer container, Type assemblyOfType)
        {
            if (container == null) throw new ArgumentNullException("container");
            if (assemblyOfType == null) throw new ArgumentNullException("assemblyOfType");

            Install(container, assemblyOfType.Assembly);
        }

        public abstract void Install(IWindsorContainer container, Assembly assembly);
    }
}
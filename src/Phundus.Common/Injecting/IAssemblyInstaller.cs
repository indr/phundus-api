namespace Phundus.Common.Injecting
{
    using System.Reflection;
    using Castle.Windsor;

    public interface IAssemblyInstaller
    {
        void Install(IWindsorContainer container, Assembly assembly);
    }
}
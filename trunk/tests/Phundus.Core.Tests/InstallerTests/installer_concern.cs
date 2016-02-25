namespace Phundus.Tests.InstallerTests
{
    using System;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Machine.Specifications;

    public class installer_concern<TInstaller> where TInstaller : IWindsorInstaller
    {
        protected static IWindsorContainer Container;
        public Establish ctx = () => { Container = new WindsorContainer(); };

        public Because of = () =>
        {
            var sut = (TInstaller) Activator.CreateInstance(typeof (TInstaller));
            Container.Install(sut);
        };
    }
}
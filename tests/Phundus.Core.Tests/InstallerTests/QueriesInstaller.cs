namespace Phundus.Tests.InstallerTests
{
    using System;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Dashboard.Querying;
    using Machine.Specifications;

    [Subject(typeof (QueriesInstaller))]
    public class when_queries_installer_is_installed : installer_concern<QueriesInstaller>
    {
        public It should_resolve_IEventsQueries_to_EventsReadModel =
            () => Container.Resolve<IEventLogQueries>().ShouldBeOfExactType<EventLogReadModel>();
    }

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
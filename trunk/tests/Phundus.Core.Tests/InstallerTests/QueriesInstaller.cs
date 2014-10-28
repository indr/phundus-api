namespace Phundus.Core.Tests.InstallerTests
{
    using System;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Common.Notifications;
    using Dashboard.Application;
    using Dashboard.Port.Adapter.Persistence.View;
    using Infrastructure;
    using Machine.Specifications;

    [Subject(typeof (QueryServiceInstaller))]
    public class when_query_service_installer_is_installed : installer_concern<QueryServiceInstaller>
    {
        public It should_resolve_IActivityQueryService_to_ActivityQueryService =
            () => Container.Resolve<IActivityQueryService>().ShouldBeOfExactType<ActivityQueryService>();
    }

    [Subject(typeof (ProjectionInstaller))]
    public class when_projection_installer_is_installed : installer_concern<ProjectionInstaller>
    {
        public It should_resolve_IDomainEventHandler_to_NHibernateActivityProjection =
            () =>
                Container.ResolveAll<IDomainEventHandler>()
                    .ShouldContain(p => p.GetType().Equals(typeof (NHibernateActivityProjection)));
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
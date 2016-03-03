namespace Phundus.Common.Tests
{
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using developwithpassion.specifications.rhinomocks;
    using Injecting;
    using Machine.Specifications;

    public class installer_concern : Observes
    {
        protected static WindsorContainer container;

        private Cleanup cleanup = () =>
            container.Dispose();

        private Establish ctx = () =>
            container = new WindsorContainer();

        protected static void register<T>() where T : class
        {
            container.Register(Component.For(typeof(T)).Instance(fake.an<T>()));
        }

        protected static T resolve<T>()
        {
            return container.Resolve<T>();
        }

        protected static T[] resolveAll<T>()
        {
            return container.ResolveAll<T>();
        }
    }

    public class assembly_installer_concern<TInstaller, TTypeOfAssembly> : installer_concern
        where TInstaller : IAssemblyInstaller, new()
    {
        private Because of = () =>
            new TInstaller().Install(container, typeof (TTypeOfAssembly).Assembly);
    }

    public class windsor_installer_concern<TInstaller> : installer_concern
        where TInstaller : IWindsorInstaller, new()
    {
        private Establish ctx = () =>
            container.AddFacility<TypedFactoryFacility>();

        private Because of = () =>
            container.Install(new TInstaller());
    }
}
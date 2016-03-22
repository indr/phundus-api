namespace Phundus.Common.Tests
{
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using developwithpassion.specifications.rhinomocks;
    using Injecting;
    using Machine.Specifications;

    public class container_concern<TClass> : Observes<TClass> where TClass : class
    {
        protected static WindsorContainer container;

        private Cleanup cleanup = () =>
            container.Dispose();

        private Establish ctx = () =>
        {
            container = new WindsorContainer();
            container.AddFacility<TypedFactoryFacility>();
        };

        protected static void register<T>() where T : class
        {
            container.Register(Component.For(typeof (T)).Instance(fake.an<T>()));
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

    public class container_concern : Observes
    {
        protected static WindsorContainer container;
        protected static IConfigurationStore configurationStore;

        private Cleanup cleanup = () =>
            container.Dispose();

        private Establish ctx = () =>
        {
            configurationStore = fake.an<IConfigurationStore>();
            container = new WindsorContainer();
            container.AddFacility<TypedFactoryFacility>();
        };

        protected static void register<T>() where T : class
        {
            container.Register(Component.For(typeof (T)).Instance(fake.an<T>()));
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

    public class installer_concern : container_concern
    {
    }

    public class assembly_installer_concern<TInstaller, TTypeOfAssembly> : installer_concern
        where TInstaller : IAssemblyInstaller, new()
    {
        private Because of = () =>
            new TInstaller().Install(container, typeof (TTypeOfAssembly).Assembly);
    }

    public class windsor_installer_concern<TInstaller> : container_concern where TInstaller : IWindsorInstaller, new()
    {
        private Because of = () =>
            new TInstaller().Install(container, configurationStore);
    }
}
namespace piNuts.phundus.Infrastructure
{
    using System;
    using System.Collections;
    using Castle.Windsor;

    public static class GlobalContainer
    {
        static IWindsorContainer _container;

        static GlobalContainer()
        {
        }

        public static IWindsorContainer Container
        {
            get
            {
                if (_container == null)
                    throw new InvalidOperationException(
                        "The container has not been initialized! Please call IoC.Initialize(container) before using it.");
                else
                    return _container;
            }
        }


        public static bool IsInitialized
        {
            get { return Infrastructure.GlobalContainer.Container != null; }
        }

        public static void Initialize(IWindsorContainer windsorContainer)
        {
            _container = windsorContainer;
        }

        public static object Resolve(Type serviceType)
        {
            return Infrastructure.GlobalContainer.Container.Resolve(serviceType);
        }

        public static object Resolve(Type serviceType, string serviceName)
        {
            return Infrastructure.GlobalContainer.Container.Resolve(serviceName, serviceType);
        }

        public static T TryResolve<T>()
        {
            return Infrastructure.GlobalContainer.TryResolve<T>(default(T));
        }

        public static T TryResolve<T>(T defaultValue)
        {
            if (!Infrastructure.GlobalContainer.Container.Kernel.HasComponent(typeof (T)))
                return defaultValue;
            else
                return Infrastructure.GlobalContainer.Container.Resolve<T>();
        }

        public static T Resolve<T>()
        {
            return Infrastructure.GlobalContainer.Container.Resolve<T>();
        }

        public static T Resolve<T>(string name)
        {
            return Infrastructure.GlobalContainer.Container.Resolve<T>(name);
        }

        public static T Resolve<T>(object argumentsAsAnonymousType)
        {
            return Infrastructure.GlobalContainer.Container.Resolve<T>(argumentsAsAnonymousType);
        }

        public static T Resolve<T>(IDictionary parameters)
        {
            return Infrastructure.GlobalContainer.Container.Resolve<T>(parameters);
        }

        public static Array ResolveAll(Type service)
        {
            return Infrastructure.GlobalContainer.Container.ResolveAll(service);
        }

        public static T[] ResolveAll<T>()
        {
            return Infrastructure.GlobalContainer.Container.ResolveAll<T>();
        }
    }
}
namespace Phundus.Specs.Services
{
    using System;
    using Castle.Windsor;
    using TechTalk.SpecFlow;

    [Binding]
    public class ApiClient
    {
        private readonly IWindsorContainer _container;

        public ApiClient(IWindsorContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");
            _container = container;
        }

        public T For<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
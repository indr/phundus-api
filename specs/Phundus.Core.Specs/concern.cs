namespace Phundus.Core.Specs
{
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Rhino.Mocks;

    // ReSharper disable once InconsistentNaming
    public abstract class concern<TClass> where TClass : class
    {
        private readonly IWindsorContainer _container;

        protected concern()
        {
            _container = new WindsorContainer();
            _container.Register(Component.For<TClass>().ImplementedBy<TClass>());
        }

        protected TClass Sut
        {
            get
            {
                return _container.Resolve<TClass>();
            }
        }

        // ReSharper disable once InconsistentNaming
        protected TDependency dependsOn<TDependency>(TDependency instance = null) where TDependency : class
        {
            instance = instance ?? MockRepository.GenerateMock<TDependency>();
            _container.Register(Component.For<TDependency>().Instance(instance));
            return instance;
        }

        protected virtual void Establish()
        {
        }
    }
}
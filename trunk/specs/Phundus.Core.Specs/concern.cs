namespace Phundus.Core.Specs.Inventory.ReservationAndAvailability
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
        protected TDependency dependsOn<TDependency>() where TDependency : class
        {
            var dependency = MockRepository.GenerateMock<TDependency>();
            _container.Register(Component.For<TDependency>().Instance(dependency));
            return dependency;
        }

        protected virtual void Establish()
        {
        }
    }
}
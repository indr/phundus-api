namespace Phundus.Core.Tests._Legacy
{
    using Castle.Windsor;
    using CommonServiceLocator.WindsorAdapter;
    using Microsoft.Practices.ServiceLocation;
    using NUnit.Framework;

    public class TestBase
    {
        private WindsorContainer _container;

        protected WindsorContainer Container
        {
            get { return _container; }
        }

        [SetUp]
        public virtual void SetUp()
        {
            _container = new WindsorContainer();
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(_container));
        }

        [TearDown]
        public virtual void TearDown()
        {
            ServiceLocator.SetLocatorProvider(null);
            _container.Dispose();
        }
    }
}
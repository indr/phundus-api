namespace Phundus.Core.Tests._Legacy
{
    using Castle.Windsor;
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
        }

        [TearDown]
        public virtual void TearDown()
        {
            _container.Dispose();
        }
    }
}
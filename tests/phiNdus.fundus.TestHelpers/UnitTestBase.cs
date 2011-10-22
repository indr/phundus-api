using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using Rhino.Commons;
using Rhino.Mocks;

namespace phiNdus.fundus.TestHelpers
{
    public class UnitTestBase
    {
        [SetUp]
        public virtual void SetUp()
        {
            var container = new WindsorContainer();
            IoC.Initialize(container);
        }

        [TearDown]
        public virtual void TearDown()
        {
        }

        protected virtual void GenerateMissingStubs()
        {
        }

        protected T GenerateAndRegisterStub<T>() where T : class
        {
            var result = MockRepository.GenerateStub<T>();
            IoC.Container.Register(Component.For<T>().Instance(result));
            return result;
        }
    }
}
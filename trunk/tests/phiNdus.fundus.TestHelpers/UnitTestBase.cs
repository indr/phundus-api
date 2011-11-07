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
            IoC.Initialize(new WindsorContainer());
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

        protected T GenerateAndRegisterMock<T>() where T : class
        {
            var result = MockRepository.GenerateStrictMock<T>();
            IoC.Container.Register(Component.For<T>().Instance(result));
            return result;
        }
    }
}
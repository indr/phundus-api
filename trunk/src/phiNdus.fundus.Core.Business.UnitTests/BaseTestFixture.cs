using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using Rhino.Commons;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Business.UnitTests
{
    internal class BaseTestFixture
    {
        [SetUp]
        public virtual void SetUp()
        {
            IoC.Initialize(new WindsorContainer());
            MockFactory = new MockRepository();
        }

        [TearDown]
        public virtual void TearDown()
        {
            IoC.Container.Dispose();
        }

        protected MockRepository MockFactory { get; set; }

        protected T CreateAndRegisterStrictMock<T>()
        {
            var result = MockFactory.StrictMock<T>();
            IoC.Container.Register(Component.For<T>().Instance(result));
            return result;
        }
    }
}
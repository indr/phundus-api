﻿using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using Rhino.Commons;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Business.UnitTests
{
    internal class BaseTestFixture
    {
        protected MockRepository MockFactory { get; set; }

        protected IUnitOfWork MockUnitOfWork { get; set; }

        [SetUp]
        public virtual void SetUp()
        {
            IoC.Initialize(new WindsorContainer());
            MockFactory = new MockRepository();

            MockUnitOfWork = MockFactory.StrictMock<IUnitOfWork>();
            UnitOfWork.RegisterGlobalUnitOfWork(MockUnitOfWork);
        }

        [TearDown]
        public virtual void TearDown()
        {
            IoC.Container.Dispose();
        }

        protected T CreateAndRegisterStrictMock<T>()
        {
            var result = MockFactory.StrictMock<T>();
            IoC.Container.Register(Component.For<T>().Instance(result));
            return result;
        }

        protected IUnitOfWork CreateAndRegisterDynamicUnitOfWorkMock()
        {
            var result = MockFactory.DynamicMock<IUnitOfWork>();
            UnitOfWork.RegisterGlobalUnitOfWork(result);
            return result;
        }
    }
}
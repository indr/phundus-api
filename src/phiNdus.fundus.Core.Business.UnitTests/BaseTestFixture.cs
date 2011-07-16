﻿using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Settings;
using Rhino.Commons;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Business.UnitTests
{
    public class BaseTestFixture
    {
        protected MockRepository MockFactory { get; set; }

        protected IUnitOfWork MockUnitOfWork { get; set; }

        [SetUp]
        public virtual void SetUp()
        {
            IoC.Initialize(new WindsorContainer());
            MockFactory = new MockRepository();
            
            MockUnitOfWork = CreateAndRegisterStrictUnitOfWorkMock();
            Settings.SetGlobalNonThreadSafeSettings(null);
        }

        [TearDown]
        public virtual void TearDown()
        {
            Settings.SetGlobalNonThreadSafeSettings(null);
            UnitOfWork.RegisterGlobalUnitOfWork(null);
            IoC.Container.Dispose();
        }

        protected T GenerateAndRegisterDynamic<T>() where T: class
        {
            var result = MockRepository.GenerateMock<T>();
            IoC.Container.Register(Component.For<T>().Instance(result));
            return result;
        }

        protected T GenerateAndRegisterStric<T>()
        {
            var result = MockRepository.GenerateStrictMock<T>();
            IoC.Container.Register(Component.For<T>().Instance(result));
            return result;
        }

        protected IUnitOfWork GenerateAndRegisterDynamicUnitOfWorkMock()
        {
            var result = MockRepository.GenerateMock<IUnitOfWork>();
            UnitOfWork.RegisterGlobalUnitOfWork(result);
            return result;
        }

        protected IUnitOfWork GenerateAndRegisterStrictUnitOfWorkMock()
        {
            var result = MockRepository.GenerateStrictMock<IUnitOfWork>();
            UnitOfWork.RegisterGlobalUnitOfWork(result);
            return result;
        }

        #region obsolete
        protected T CreateAndRegisterDynamicMock<T>() where T : class
        {
            var result = MockFactory.DynamicMock<T>();
            IoC.Container.Register(Component.For<T>().Instance(result));
            return result;
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

        protected IUnitOfWork CreateAndRegisterStrictUnitOfWorkMock()
        {
            var result = MockFactory.StrictMock<IUnitOfWork>();
            UnitOfWork.RegisterGlobalUnitOfWork(result);
            return result;
        }
        #endregion
    }
}
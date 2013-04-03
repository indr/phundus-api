using System;
using Castle.MicroKernel.Registration;
using NUnit.Framework;
using Rhino.Commons;
using Rhino.Mocks;

namespace phiNdus.fundus.TestHelpers.TestBases
{
    using phiNdus.fundus.Domain;
    using piNuts.phundus.Infrastructure;

    public class UnitTestBase<TSut> : TestBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            Obsolete_MockFactory = new MockRepository();
            MockUnitOfWork = Obsolete_CreateAndRegisterStrictUnitOfWorkMock();
        }

        protected TSut Sut { get; set; }
        protected IUnitOfWork MockUnitOfWork { get; set; }

        protected virtual void GenerateMissingStubs()
        {
        }

        protected T GenerateAndRegisterStub<T>() where T : class
        {
            var result = MockRepository.GenerateStub<T>();
            GlobalContainer.Container.Register(Component.For<T>().Instance(result));
            return result;
        }

        protected T GenerateAndRegisterMock<T>() where T : class
        {
            var result = MockRepository.GenerateStrictMock<T>();
            GlobalContainer.Container.Register(Component.For<T>().Instance(result));
            return result;
        }

        protected IUnitOfWork GenerateAndRegisterMockUnitOfWork()
        {
            var result = MockRepository.GenerateMock<IUnitOfWork>();
            UnitOfWork.RegisterGlobalUnitOfWork(result);
            return result;
        }

        protected IUnitOfWork GenerateAndRegisterStubUnitOfWork()
        {
            var result = MockRepository.GenerateStub<IUnitOfWork>();
            UnitOfWork.RegisterGlobalUnitOfWork(result);
            return result;
        }

        #region Obsolete
        protected MockRepository Obsolete_MockFactory { get; set; }
        protected T Obsolete_CreateAndRegisterDynamicMock<T>() where T : class
        {
            var result = Obsolete_MockFactory.DynamicMock<T>();
            GlobalContainer.Container.Register(Component.For<T>().Instance(result));
            return result;
        }

        protected T Obsolete_CreateAndRegisterStrictMock<T>() where T : class
        {
            var result = Obsolete_MockFactory.StrictMock<T>();
            GlobalContainer.Container.Register(Component.For<T>().Instance(result));
            return result;
        }

        protected IUnitOfWork Obsolete_CreateAndRegisterDynamicUnitOfWorkMock()
        {
            var result = Obsolete_MockFactory.DynamicMock<IUnitOfWork>();
            UnitOfWork.RegisterGlobalUnitOfWork(result);
            return result;
        }

        protected IUnitOfWork Obsolete_CreateAndRegisterStrictUnitOfWorkMock()
        {
            var result = Obsolete_MockFactory.StrictMock<IUnitOfWork>();
            UnitOfWork.RegisterGlobalUnitOfWork(result);
            return result;
        }
        #endregion
    }
}
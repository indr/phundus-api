using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Settings;
using Rhino.Commons;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Business.UnitTests
{
    public class BaseTestFixture
    {
        protected IUnitOfWork MockUnitOfWork { get; set; }

        [SetUp]
        public virtual void SetUp()
        {
            IoC.Initialize(new WindsorContainer());
            Obsolete_MockFactory = new MockRepository();
            
            MockUnitOfWork = Obsolete_CreateAndRegisterStrictUnitOfWorkMock();
            Settings.SetGlobalNonThreadSafeSettings(null);
        }

        [TearDown]
        public virtual void TearDown()
        {
            Settings.SetGlobalNonThreadSafeSettings(null);
            UnitOfWork.RegisterGlobalUnitOfWork(null);
            IoC.Container.Dispose();
        }

        protected T GenerateAndRegisterMock<T>() where T : class
        {
            var result = MockRepository.GenerateMock<T>();
            IoC.Container.Register(Component.For<T>().Instance(result));
            return result;
        }

        protected T GenerateAndRegisterStub<T>() where T : class
        {
            var result = MockRepository.GenerateStub<T>();
            IoC.Container.Register(Component.For<T>().Instance(result));
            return result;
        }

        protected T GenerateAndRegisterStrictMock<T>()
        {
            var result = MockRepository.GenerateStrictMock<T>();
            IoC.Container.Register(Component.For<T>().Instance(result));
            return result;
        }

        protected IUnitOfWork GenerateAndRegisterMockUnitOfWork()
        {
            var result = MockRepository.GenerateMock<IUnitOfWork>();
            UnitOfWork.RegisterGlobalUnitOfWork(result);
            return result;
        }

        protected IUnitOfWork GenerateAndRegisterStrictMockUnitOfWork()
        {
            var result = MockRepository.GenerateStrictMock<IUnitOfWork>();
            UnitOfWork.RegisterGlobalUnitOfWork(result);
            return result;
        }


        #region obsolete
        protected MockRepository Obsolete_MockFactory { get; set; }
        protected T Obsolete_CreateAndRegisterDynamicMock<T>() where T : class
        {
            var result = Obsolete_MockFactory.DynamicMock<T>();
            IoC.Container.Register(Component.For<T>().Instance(result));
            return result;
        }

        protected T Obsolete_CreateAndRegisterStrictMock<T>()
        {
            var result = Obsolete_MockFactory.StrictMock<T>();
            IoC.Container.Register(Component.For<T>().Instance(result));
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
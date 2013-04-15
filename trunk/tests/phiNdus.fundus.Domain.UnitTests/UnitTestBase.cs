using System;
using Castle.MicroKernel.Registration;
using NUnit.Framework;
using Rhino.Mocks;

namespace phiNdus.fundus.TestHelpers.TestBases
{
    using Microsoft.Practices.ServiceLocation;
    using phiNdus.fundus.Domain;
    using phiNdus.fundus.Domain.UnitTests;
    using piNuts.phundus.Infrastructure;
    using piNuts.phundus.Infrastructure.Obsolete;

    public class UnitTestBase<TSut> : TestBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            Obsolete_MockFactory = new MockRepository();
            
        }

        protected TSut Sut { get; set; }
        

        protected virtual void GenerateMissingStubs()
        {
        }

        protected T GenerateAndRegisterStub<T>() where T : class
        {
            var result = MockRepository.GenerateStub<T>();
            Container.Register(Component.For<T>().Instance(result));
            return result;
        }

        protected T GenerateAndRegisterMock<T>() where T : class
        {
            var result = MockRepository.GenerateStrictMock<T>();
            Container.Register(Component.For<T>().Instance(result));
            return result;
        }

        #region Obsolete
        protected MockRepository Obsolete_MockFactory { get; set; }
        protected T Obsolete_CreateAndRegisterDynamicMock<T>() where T : class
        {
            var result = Obsolete_MockFactory.DynamicMock<T>();
            Container.Register(Component.For<T>().Instance(result));
            return result;
        }

        protected T Obsolete_CreateAndRegisterStrictMock<T>() where T : class
        {
            var result = Obsolete_MockFactory.StrictMock<T>();
            Container.Register(Component.For<T>().Instance(result));
            return result;
        }

        #endregion
    }
}
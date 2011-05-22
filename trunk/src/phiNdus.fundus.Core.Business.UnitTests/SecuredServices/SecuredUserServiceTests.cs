using System;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.SecuredServices;
using phiNdus.fundus.Core.Business.Security;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Business.UnitTests.SecuredServices
{
    [TestFixture]
    internal class SecuredUserServiceTests : BaseTestFixture
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            MockUserRepository = CreateAndRegisterStrictMock<IUserRepository>();
            Sut = new SecuredUserService();
        }

        #endregion

        protected IUserRepository MockUserRepository { get; set; }
        protected SecuredUserService Sut { get; set; }

        [Test]
        public void GetUser_with_invalid_sessionKey_throws()
        {
            using (MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindBySessionKey("this.key.is.not.valid")).Return(null);
                Expect.Call(() => MockUnitOfWork.Dispose());
            }

            using (MockFactory.Playback())
            {
                Assert.Throws<InvalidSessionKeyException>(() => Sut.GetUser("this.key.is.not.valid", ""));
            }
        }

        [Test]
        public void GetUser_with_sessionKey_null_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Sut.GetUser(null, ""));
            Assert.That(ex.ParamName, Is.EqualTo("key"));
        }

        [Test]
        public void UpdateUser_with_invalid_sessionKey_throws()
        {
            using (MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindBySessionKey("this.key.is.not.valid")).Return(null);
                Expect.Call(() => MockUnitOfWork.Dispose());
            }

            using (MockFactory.Playback())
            {
                Assert.Throws<InvalidSessionKeyException>(() => Sut.UpdateUser("this.key.is.not.valid", null));
            }
        }

        [Test]
        public void UpdateUser_with_sessionKey_null_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Sut.UpdateUser(null, null));
            Assert.That(ex.ParamName, Is.EqualTo("key"));
        }
    }
}
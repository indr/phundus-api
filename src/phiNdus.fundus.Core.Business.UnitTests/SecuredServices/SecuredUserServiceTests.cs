using System;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.SecuredServices;
using phiNdus.fundus.Core.Business.Security;
using phiNdus.fundus.Core.Business.Services;
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
        [ExpectedException(typeof (InvalidSessionKeyException))]
        public void GetUser_with_invalid_sessionKey_throws()
        {
            using (MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindBySessionKey("this.key.is.not.valid")).Return(null);
            }

            using (MockFactory.Playback())
            {
                Sut.GetUser("this.key.is.not.valid", "");
            }
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException), ExpectedMessage = "Parameter name: key",
            MatchType = MessageMatch.Contains)]
        public void GetUser_with_sessionKey_null_throws()
        {
            Sut.GetUser(null, "");
        }

        [Test]
        [ExpectedException(typeof (InvalidSessionKeyException))]
        public void UpdateUser_with_invalid_sessionKey_throws()
        {
            using (MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindBySessionKey("this.key.is.not.valid")).Return(null);
            }

            using (MockFactory.Playback())
            {
                Sut.UpdateUser("this.key.is.not.valid", null);
            }
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException), ExpectedMessage = "Parameter name: key",
            MatchType = MessageMatch.Contains)]
        public void UpdateUser_with_sessionKey_null_throws()
        {
            Sut.UpdateUser(null, null);
        }
    }
}
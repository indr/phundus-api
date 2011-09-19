using System;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Security;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Business.UnitTests.Security
{
    [TestFixture]
    public class SecuritySessionTests : BaseTestFixture
    {
        #region Setup/Teardown

        public override void SetUp()
        {
            base.SetUp();

            MockUserRepository = Obsolete_CreateAndRegisterStrictMock<IUserRepository>();
        }

        #endregion

        private IUserRepository MockUserRepository { get; set; }

        [Test]
        public void FromKey_returns_session_with_user_and_key()
        {
            var user = new User();

            using (Obsolete_MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindBySessionKey("ABCD")).Return(user);
                Expect.Call(() => MockUnitOfWork.Dispose());
            }

            using (Obsolete_MockFactory.Playback())
            {
                var session = SecuritySession.FromKey("ABCD");
                Assert.That(session, Is.Not.Null);
                Assert.That(session.User, Is.EqualTo(user));
                Assert.That(session.Key, Is.EqualTo("ABCD"));
            }
        }

        [Test]
        public void FromKey_with_invalid_throws()
        {
            using (Obsolete_MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindBySessionKey("this.is.an.invalid.key")).Return(null);
                Expect.Call(() => MockUnitOfWork.Dispose());
            }

            using (Obsolete_MockFactory.Playback())
            {
                Assert.Throws<InvalidSessionKeyException>(() => SecuritySession.FromKey("this.is.an.invalid.key"));
            }
        }

        [Test]
        public void FromKey_with_null_throws()
        {
            Assert.Throws<ArgumentNullException>(() => SecuritySession.FromKey(null));
        }
    }
}
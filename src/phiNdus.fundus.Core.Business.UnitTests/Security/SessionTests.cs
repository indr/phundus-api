using System;
using Castle.MicroKernel.Registration;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Security;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Business.UnitTests.Security
{
    [TestFixture]
    internal class SessionTests : BaseTestFixture
    {
        public override void SetUp()
        {
            base.SetUp();

            MockUserRepository = CreateAndRegisterStrictMock<IUserRepository>();
        }

        private IUserRepository MockUserRepository { get; set; }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FromKey_with_null_throws()
        {
            Session.FromKey(null);
        }

        [Test]
        [ExpectedException(typeof(InvalidSessionKeyException))]
        public void FromKey_with_invalid_throws()
        {
            using (MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindBySessionKey("this.is.an.invalid.key")).Return(null);
            }

            using (MockFactory.Playback()) {
                Session.FromKey("this.is.an.invalid.key");
            }
        }

        [Test]
        public void FromKey_returns_session_with_user_and_key()
        {
            var user = new User();

            using (MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindBySessionKey("ABCD")).Return(user);
            }

            using (MockFactory.Playback())
            {
                var session = Session.FromKey("ABCD");
                Assert.That(session, Is.Not.Null);
                Assert.That(session.User, Is.EqualTo(user));
                Assert.That(session.Key, Is.EqualTo("ABCD"));
            }
        }
    }
}
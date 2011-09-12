using System;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.SecuredServices;
using phiNdus.fundus.Core.Business.Security;

namespace phiNdus.fundus.Core.Business.IntegrationTests.SecuredServices
{
    [TestFixture]
    public class SecuredUserServiceTests : BaseTestFixture
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Sut = new SecuredUserService();
        }

        #endregion

        protected IUserService Sut { get; set; }

        private string GetNewSessionKey()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        [Test]
        public void GetUser_own_with_user_roll()
        {
            var sessionKey = GetNewSessionKey();
            var valid = Sut.ValidateUser(sessionKey, "robin.scherbatsky@example.com", "robin");
            Assert.That(valid, Is.True);

            var actual = Sut.GetUser(sessionKey, "robin.scherbatsky@example.com");
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Email, Is.EqualTo("robin.scherbatsky@example.com"));
        }

        [Test]
        public void GetUser_other_with_user_roll_throws()
        {
            var sessionKey = GetNewSessionKey();
            var valid = Sut.ValidateUser(sessionKey, "robin.scherbatsky@example.com", "robin");
            Assert.That(valid, Is.True);

            Assert.Throws<AuthorizationException>(() => Sut.GetUser(sessionKey, "barney.stinson@example.com"));
        }

        [Test]
        public void GetUser_other_with_administrator_roll()
        {
            var sessionKey = GetNewSessionKey();
            var valid = Sut.ValidateUser(sessionKey, "barney.stinson@example.com", "barney");
            Assert.That(valid, Is.True);

            var actual = Sut.GetUser(sessionKey, "robin.scherbatsky@example.com");
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Email, Is.EqualTo("robin.scherbatsky@example.com"));
        }
    }
}
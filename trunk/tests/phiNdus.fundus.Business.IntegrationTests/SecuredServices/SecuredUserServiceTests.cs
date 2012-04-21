using System;
using NUnit.Framework;
using phiNdus.fundus.Business.SecuredServices;
using phiNdus.fundus.Business.Security;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.TestHelpers.Builders;

namespace phiNdus.fundus.Business.IntegrationTests.SecuredServices
{
    [TestFixture]
    public class SecuredUserServiceTests : BusinessComponentTestBase<IUserService>
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            Sut = new SecuredUserService();
        }

        #endregion

        [Test]
        public void GetUser_other_with_administrator_roll()
        {
            // Arrange
            var adminSessionKey = "";
            var otherUserMail = "";
            Transactional(() =>
                              {
                                  adminSessionKey = new UserBuilder().Admin().Build().Membership.SessionKey;
                                  otherUserMail = new UserBuilder().Build().Membership.Email;
                              });

            // Act
            var actual = Sut.GetUser(adminSessionKey, otherUserMail);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Email, Is.EqualTo(otherUserMail));
        }

        [Test]
        public void GetUser_other_with_user_roll_throws()
        {
            // Arrange
            var sessionKey = "";
            Transactional(() => { sessionKey = new UserBuilder().Build().Membership.SessionKey; });

            // Act/Assert
            Assert.Throws<AuthorizationException>(() => Sut.GetUser(sessionKey, "other@example.com"));
        }

        [Test]
        public void GetUser_own_with_user_roll()
        {
            // Arrange
            var sessionKey = "";
            Transactional(() => { sessionKey = new UserBuilder().Build().Membership.SessionKey; });

            // Act
            var actual = Sut.GetUser(sessionKey, "user@example.com");

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.RoleId, Is.EqualTo(Role.User.Id));
            Assert.That(actual.Email, Is.EqualTo("user@example.com"));
        }
    }
}
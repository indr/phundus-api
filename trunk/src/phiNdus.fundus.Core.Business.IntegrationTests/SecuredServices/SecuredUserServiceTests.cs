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

        [Test]
        public void GetUser_own_with_user_roll()
        {
            var sessionKey = Sut.ValidateUser("robin.scherbatsky@example.com", "robin");
            Assert.That(sessionKey, Is.Not.Null);

            var actual = Sut.GetUser(sessionKey, "robin.scherbatsky@example.com");
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Email, Is.EqualTo("robin.scherbatsky@example.com"));
        }

        [Test]
        public void GetUser_other_with_user_roll_throws()
        {
            var sessionKey = Sut.ValidateUser("robin.scherbatsky@example.com", "robin");
            Assert.That(sessionKey, Is.Not.Null);

            Assert.Throws<AuthorizationException>(() => Sut.GetUser(sessionKey, "barney.stinson@example.com"));
        }

        [Test]
        public void GetUser_other_with_administrator_roll()
        {
            var sessionKey = Sut.ValidateUser("barney.stinson@example.com", "barney");
            Assert.That(sessionKey, Is.Not.Null);

            var actual = Sut.GetUser(sessionKey, "robin.scherbatsky@example.com");
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Email, Is.EqualTo("robin.scherbatsky@example.com"));
        }
    }
}
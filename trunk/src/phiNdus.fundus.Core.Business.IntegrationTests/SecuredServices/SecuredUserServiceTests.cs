using NUnit.Framework;
using phiNdus.fundus.Core.Business.SecuredServices;

namespace phiNdus.fundus.Core.Business.IntegrationTests.SecuredServices
{
    [TestFixture]
    internal class SecuredUserServiceTests : BaseTestFixture
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
        public void GetUser_returns_dto()
        {
            var sessionKey = Sut.ValidateUser("ted.mosby@example.com", "1234");
            Assert.That(sessionKey, Is.Not.Null);

            var actual = Sut.GetUser(sessionKey, "ted.mosby@example.com");
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Email, Is.EqualTo("ted.mosby@example.com"));
        }
    }
}
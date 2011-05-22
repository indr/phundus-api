using NUnit.Framework;
using phiNdus.fundus.Core.Business.SecuredServices;

namespace phiNdus.fundus.Core.Business.IntegrationTests.SecuredServices
{
    [TestFixture]
    class SecuredUserServiceTests : BaseTestFixture
    {
        [SetUp]
        public void SetUp()
        {
            Sut = new SecuredUserService();
        }

        protected IUserService Sut { get; set; }

        [Test]
        public void GetUser_returns_dto()
        {
            Assert.Ignore("IUserService.ValidateUser() muss Session-Key liefern.");
            var key = Sut.ValidateUser("ted.mosby@example.com", "1234");

            var actual = Sut.GetUser(null, "ted.mosby@example.com");
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Email, Is.EqualTo("ted.mosby@example.com"));
        }
    }
}

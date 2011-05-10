using NUnit.Framework;
using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Business.IntegrationTests
{
    [TestFixture]
    public class UserServiceTests : BaseTestFixture
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _svc = new UserService();
        }

        #endregion

        private IUserService _svc;

        [Test]
        public void GetUser_returns_dto()
        {
            UserDto dto = _svc.GetUser("ted.mosby@example.com");
            Assert.That(dto, Is.Not.Null);
            Assert.That(dto.Email, Is.EqualTo("ted.mosby@example.com"));
        }
    }
}
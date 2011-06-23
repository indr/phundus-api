using System.Net.Mail;
using System.Threading;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.IntegrationTests.TestHelpers;
using phiNdus.fundus.Core.Business.Services;

namespace phiNdus.fundus.Core.Business.IntegrationTests.Services
{
    [TestFixture]
    public class UserServiceTests : BaseTestFixture
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Sut = new UserService();
        }

        #endregion

        private UserService Sut { get; set; }

        [Test]
        public void CreateUser_returns_dto_of_new_user()
        {
			Assert.Ignore("E-Mail-Server auf phindus.ch installieren und nutzen...");
            try
            {
                Thread.Sleep(Pop3.SendDelay);
                var dto = Sut.CreateUser("nirvana@indr.ch", "1234");

                Assert.That(dto.Email, Is.EqualTo("nirvana@indr.ch"));
                Assert.That(dto.Id, Is.GreaterThan(0));
                Assert.That(dto.IsApproved, Is.False);
            }
            catch (SmtpException ex)
            {
                if (ex.Message.Contains("because the server is too busy."))
                    Assert.Ignore(ex.Message);
                else
                    throw;
            }
        }

        [Test]
        public void GetUser_returns_dto()
        {
            var dto = Sut.GetUser("ted.mosby@example.com");
            Assert.That(dto, Is.Not.Null);
            Assert.That(dto.Email, Is.EqualTo("ted.mosby@example.com"));
        }
    }
}
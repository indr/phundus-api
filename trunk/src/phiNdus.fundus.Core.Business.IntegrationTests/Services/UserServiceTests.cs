using System;
using System.Threading;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.IntegrationTests.Helpers;
using phiNdus.fundus.Core.Business.Services;

namespace phiNdus.fundus.Core.Business.IntegrationTests.Services
{
    [TestFixture]
    internal class UserServiceTests : BaseTestFixture
    {
        #region SetUp/TearDown

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
            var dto = Sut.CreateUser("fundus-sys-test-2@indr.ch", "1234");

            Assert.That(dto.Email, Is.EqualTo("fundus-sys-test-2@indr.ch"));
            Assert.That(dto.Id, Is.GreaterThan(0));
            Assert.That(dto.IsApproved, Is.False);

            Thread.Sleep(Pop3.RetrieveDelay);

            var mail = Pop3.RetrieveMail("mail.indr.ch", "fundus-sys-test-2@indr.ch", "phiNdus", "[fundus] User Account Validation");
            Assert.That(mail, Is.Not.Null, "Could not retrieve mail.");
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
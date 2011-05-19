using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.IntegrationTests.Repositories
{
    internal class SettingRepositoryTests : UnitOfWorkEnsuredTestFixture
    {
        private ISettingRepository Sut { get; set; }

        [SetUp]
        public void SetUp()
        {
            Sut = new SettingRepository();
        }

        [Test]
        public void Can_find_by_key()
        {
            using (UnitOfWork.Start())
            {
                var fromRepo = Sut.FindByKey("mail.smtp.host");
                Assert.That(fromRepo, Is.Not.Null);
                Assert.That(fromRepo.Key, Is.EqualTo("mail.smtp.host"));
                Assert.That(fromRepo.String, Is.Not.Null);
            }
        }
    }
}
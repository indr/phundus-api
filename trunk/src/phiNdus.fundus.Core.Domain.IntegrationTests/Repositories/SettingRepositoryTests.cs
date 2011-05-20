using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;
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

        [Test]
        public void Can_find_all_in_keyspace()
        {
            using (UnitOfWork.Start())
            {
                var fromRepo = Sut.FindByKeyspace("mail.smtp");
                Assert.That(fromRepo, Is.Not.Null);
                CollectionAssert.Contains(fromRepo.Keys, "host" );
                CollectionAssert.Contains(fromRepo.Keys, "user-name");
                CollectionAssert.Contains(fromRepo.Keys, "password");
                CollectionAssert.Contains(fromRepo.Keys, "from");
            }
        }
    }
}
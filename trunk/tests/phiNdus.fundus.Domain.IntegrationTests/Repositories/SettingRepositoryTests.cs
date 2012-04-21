using System;
using NUnit.Framework;
using phiNdus.fundus.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Domain.IntegrationTests.Repositories
{
    [TestFixture]
    public class SettingRepositoryTests : SettingsTestFixture<ISettingRepository>
    {
        protected override ISettingRepository CreateSut()
        {
            return IoC.Resolve<ISettingRepository>();
        }

        [Test]
        public void CanFindBykey()
        {
            InsertSetting("keyspace.key1", "value1");
            InsertSetting("keyspace.key2", "value2");

            using (UnitOfWork.Start())
            {
                var fromRepo = Sut.FindByKey("keyspace.key1");
                Assert.That(fromRepo, Is.Not.Null);
                Assert.That(fromRepo.Key, Is.EqualTo("keyspace.key1"));
                Assert.That(fromRepo.StringValue, Is.EqualTo("value1"));
            }
        }

        [Test]
        public void CanFindAllInKeyspace()
        {
            InsertSetting("keyspace.key1", "value1");
            InsertSetting("keyspace.key2", "value2");

            using (UnitOfWork.Start())
            {
                var fromRepo = Sut.FindByKeyspace("keyspace");
                Assert.That(fromRepo, Is.Not.Null);
                CollectionAssert.Contains(fromRepo.Keys, "key1");
                CollectionAssert.Contains(fromRepo.Keys, "key2");
            }
        }
    }
}
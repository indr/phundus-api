using NUnit.Framework;

namespace phiNdus.fundus.Core.Domain.UnitTests
{
    [TestFixture]
    class TestBaseEntity
    {
        [Test]
        public void Can_create()
        {
            var entity = new BaseEntity();
        }

        [Test]
        public void Get_Id()
        {
            var entity = new BaseEntity();

            Assert.That(entity.Id, Is.EqualTo(0));
        }

        [Test]
        public void Get_Version()
        {
            var entity = new BaseEntity();

            Assert.That(entity.Version, Is.EqualTo(0));
        }
    }
}

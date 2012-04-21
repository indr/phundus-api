using NUnit.Framework;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities
{
    [TestFixture]
    public class EntityTests
    {
        private class Entity : Domain.Entities.Entity
        {
        }

        public void Get_Id()
        {
            var entity = new Entity();
            Assert.That(entity.Id, Is.EqualTo(0));
        }

        [Test]
        public void Get_Version()
        {
            var entity = new Entity();
            Assert.That(entity.Version, Is.EqualTo(0));
        }
    }
}
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities
{
    [TestFixture]
    internal class BaseEntityTests
    {
        private class InstantiableBaseEntity : BaseEntity
        {
        }

        [Test]
        public void Get_Id()
        {
            var entity = new InstantiableBaseEntity();
            Assert.That(entity.Id, Is.EqualTo(0));
        }

        [Test]
        public void Get_Version()
        {
            var entity = new InstantiableBaseEntity();
            Assert.That(entity.Version, Is.EqualTo(0));
        }
    }
}
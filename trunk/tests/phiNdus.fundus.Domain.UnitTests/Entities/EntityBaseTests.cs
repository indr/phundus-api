using NUnit.Framework;

namespace phiNdus.fundus.Domain.UnitTests.Entities
{
    [TestFixture]
    public class EntityBaseTests
    {
        private class EntityBase : Domain.Entities.EntityBase
        {
        }

        [Test]
        public void Id_AfterInstantiation_ShouldEqual0()
        {
            // Arrange
            var sut = new EntityBase();

            // Act

            // Assert
            Assert.That(sut.Id, Is.EqualTo(0));
        }

        [Test]
        public void Version_AfterInstantiation_ShouldEqual0()
        {
            // Arrange
            var sut = new EntityBase();

            // Act

            // Assert
            Assert.That(sut.Version, Is.EqualTo(0));
        }
    }
}
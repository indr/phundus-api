namespace phiNdus.fundus.Domain.UnitTests.Entities
{
    using NUnit.Framework;
    using Phundus.Core.Ddd;
    using Phundus.Core.InventoryCtx;
    using Phundus.Core.InventoryCtx.Model;

    [TestFixture]
    public class FieldDefinitionTests
    {
        [Test]
        public void Can_create()
        {
            new FieldDefinition();
        }

        [Test]
        public void Can_get_IsSystem()
        {
            var sut = new FieldDefinition(1, 2, "Name", DataType.Text, true);
            Assert.That(sut.IsSystem, Is.True);
        }

        [Test]
        public void Can_get_Name()
        {
            var sut = new FieldDefinition(1, "Name", DataType.Text);
            Assert.That(sut.Name, Is.EqualTo("Name"));
        }

        [Test]
        public void Can_get_Type()
        {
            var sut = new FieldDefinition(1, "Name", DataType.Text);
            Assert.That(sut.DataType, Is.EqualTo(DataType.Text));
        }

        [Test]
        public void Is_derived_from_BaseEntity()
        {
            var sut = new FieldDefinition();
            Assert.That(sut, Is.InstanceOf(typeof (EntityBase)));
        }
    }
}
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Business.UnitTests.Dto
{
    [TestFixture]
    public class FieldDefinitionDtoTests
    {
        [Test]
        public void Can_create()
        {
            var sut = new FieldDefinitionDto();
            Assert.That(sut, Is.Not.Null);
        }

        [Test]
        public void Can_get_and_set_Caption()
        {
            var sut = new FieldDefinitionDto();
            sut.Caption = "Bezeichnung";
            Assert.That(sut.Caption, Is.EqualTo("Bezeichnung"));
        }

        [Test]
        public void Can_get_and_set_DataType()
        {
            var sut = new FieldDefinitionDto();
            sut.DataType = FieldDataType.Text;
            Assert.That(sut.DataType, Is.EqualTo(FieldDataType.Text));
        }

        [Test]
        public void Can_get_and_set_Id()
        {
            var sut = new FieldDefinitionDto();
            sut.Id = 1;
            Assert.That(sut.Id, Is.EqualTo(1));
        }

        [Test]
        public void Can_get_and_set_Version()
        {
            var sut = new FieldDefinitionDto();
            sut.Version = 1;
            Assert.That(sut.Version, Is.EqualTo(1));
        }

        [Test]
        public void Can_get_and_set_IsSystem()
        {
            var sut = new FieldDefinitionDto();
            sut.IsSystem = true;
            Assert.That(sut.IsSystem, Is.True);
        }
    }
}
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Business.UnitTests.Dto
{
    [TestFixture]
    public class PropertyDtoTests
    {
        [Test]
        public void Can_create()
        {
            var sut = new PropertyDto();
            Assert.That(sut, Is.Not.Null);
        }

        [Test]
        public void Can_get_and_set_Caption()
        {
            var sut = new PropertyDto();
            sut.Caption = "Bezeichnung";
            Assert.That(sut.Caption, Is.EqualTo("Bezeichnung"));
        }

        [Test]
        public void Can_get_and_set_DataType()
        {
            var sut = new PropertyDto();
            sut.DataType = PropertyDataType.Text;
            Assert.That(sut.DataType, Is.EqualTo(PropertyDataType.Text));
        }

        [Test]
        public void Can_get_and_set_Id()
        {
            var sut = new PropertyDto();
            sut.Id = 1;
            Assert.That(sut.Id, Is.EqualTo(1));
        }

        [Test]
        public void Can_get_and_set_Version()
        {
            var sut = new PropertyDto();
            sut.Version = 1;
            Assert.That(sut.Version, Is.EqualTo(1));
        }
    }
}
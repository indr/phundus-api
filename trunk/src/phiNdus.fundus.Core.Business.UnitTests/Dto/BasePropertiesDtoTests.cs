using NUnit.Framework;
using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Business.UnitTests.Dto
{
    [TestFixture]
    public class BasePropertiesDtoTests
    {
        [Test]
        public void Can_create()
        {
            var sut = new BasePropertiesDto();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Properties, Is.Not.Null);
            Assert.That(sut.Properties, Has.Count.EqualTo(0));
        }

        [Test]
        public void Can_add_DtoProperty()
        {
            var sut = new BasePropertiesDto();

            var dtoProperty = new DtoProperty();
            sut.AddProperty(dtoProperty);

            Assert.That(sut.Properties, Contains.Item(dtoProperty));
        }
    }
}
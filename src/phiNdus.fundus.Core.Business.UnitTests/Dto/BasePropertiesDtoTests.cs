using NUnit.Framework;
using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Business.UnitTests.Dto
{
    [TestFixture]
    public class BasePropertiesDtoTests
    {
        [Test]
        public void Can_add_DtoProperty()
        {
            var sut = new BasePropertiesDto();

            var dtoProperty = new DtoProperty();
            sut.AddProperty(dtoProperty);

            Assert.That(sut.Properties, Contains.Item(dtoProperty));
        }

        [Test]
        public void Can_create()
        {
            var sut = new BasePropertiesDto();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Properties, Is.Not.Null);
            Assert.That(sut.Properties, Has.Count.EqualTo(0));
        }

        [Test]
        public void Can_remove_DtoProperty_by_PropertyId()
        {
            var sut = new BasePropertiesDto();
            var dtoProperty = new DtoProperty
                                  {
                                      PropertyId = 1
                                  };
            sut.AddProperty(dtoProperty);
            Assert.That(sut.Properties, Contains.Item(dtoProperty));

            sut.RemoveProperty(1);
            Assert.That(sut.Properties, Has.Count.EqualTo(0));
        }

        [Test]
        public void Can_remove_DtoProperty_by_reference()
        {
            var sut = new BasePropertiesDto();
            var dtoProperty = new DtoProperty();
            sut.AddProperty(dtoProperty);

            Assert.That(sut.Properties, Contains.Item(dtoProperty));

            sut.RemoveProperty(dtoProperty);

            Assert.That(sut.Properties, Has.Count.EqualTo(0));
        }

        [Test]
        public void GetPropertyValue_by_PropertyId()
        {
            var sut = new BasePropertiesDto();
            var dtoProperty = new DtoProperty
                                  {
                                      PropertyId = 1,
                                      Value = "Value"
                                  };
            
            sut.AddProperty(dtoProperty);

            var actual = sut.GetPropertyValue(1);

            Assert.That(actual, Is.EqualTo("Value"));
        }

        [Test]
        public void GetPropertyValue_by_PropertyId_with_property_not_defined_returns_null()
        {
            var sut = new BasePropertiesDto();

            var actual = sut.GetPropertyValue(1);

            Assert.That(actual, Is.Null);

        }
    }
}
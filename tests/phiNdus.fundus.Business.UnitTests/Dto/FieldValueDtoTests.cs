using NUnit.Framework;
using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Business.UnitTests.Dto
{
    [TestFixture]
    public class FieldValueDtoTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Sut = new FieldValueDto();
        }

        #endregion

        protected FieldValueDto Sut { get; set; }

        [Test]
        public void Can_create()
        {
            var sut = new FieldValueDto();
            Assert.That(sut, Is.Not.Null);
        }

        [Test]
        public void Can_get_and_set_Caption()
        {
            Sut.Caption = "Bezeichnung";
            Assert.That(Sut.Caption, Is.EqualTo("Bezeichnung"));
        }

        [Test]
        public void Can_get_and_set_DataType()
        {
            Sut.DataType = FieldDataType.Text;
            Assert.That(Sut.DataType, Is.EqualTo(FieldDataType.Text));
        }

        [Test]
        public void Can_get_and_set_PropertyId()
        {
            Sut.PropertyId = 1;
            Assert.That(Sut.PropertyId, Is.EqualTo(1));
        }

        [Test]
        public void Can_get_and_set_Value()
        {
            var value = new object();
            Sut.Value = value;
            Assert.That(Sut.Value, Is.SameAs(value));
        }

        [Test]
        public void Can_get_and_set_ValueId()
        {
            Sut.ValueId = 1;
            Assert.That(Sut.ValueId, Is.EqualTo(1));
        }

        [Test]
        public void Can_get_and_set_IsDiscriminator()
        {
            Assert.That(Sut.IsDiscriminator, Is.False);
            Sut.IsDiscriminator = true;
            Assert.That(Sut.IsDiscriminator, Is.True);
        }
    }
}
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Business.UnitTests.Dto
{
    [TestFixture]
    public class DtoPropertyTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Sut = new DtoProperty();
        }

        #endregion

        protected DtoProperty Sut { get; set; }

        [Test]
        public void Can_create()
        {
            var sut = new DtoProperty();
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
            Sut.DataType = DtoPropertyDataType.Text;
            Assert.That(Sut.DataType, Is.EqualTo(DtoPropertyDataType.Text));
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
    }
}
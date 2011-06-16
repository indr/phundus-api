using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities
{
    [TestFixture]
    public class SettingTests
    {
        [Test]
        public void Can_create()
        {
            new Setting();
        }

        [Test]
        public void Can_get_key()
        {
            var setting = new Setting();
            Assert.That(setting.Key, Is.Null);
        }

        [Test]
        public void Can_set_and_get_Boolean()
        {
            var setting = new Setting();
            Assert.That(setting.BooleanValue, Is.Null);
            setting.BooleanValue = true;
            Assert.That(setting.BooleanValue, Is.True);
        }

        [Test]
        public void Can_set_and_get_Decimal()
        {
            var setting = new Setting();
            Assert.That(setting.DecimalValue, Is.Null);
            setting.DecimalValue = 1.1d;
            Assert.That(setting.DecimalValue, Is.EqualTo(1.1d));
        }

        [Test]
        public void Can_set_and_get_Integer()
        {
            var setting = new Setting();
            Assert.That(setting.IntegerValue, Is.Null);
            setting.IntegerValue = 1;
            Assert.That(setting.IntegerValue, Is.EqualTo(1));
        }

        [Test]
        public void Can_set_and_get_String()
        {
            var setting = new Setting();
            Assert.That(setting.StringValue, Is.Null);
            setting.StringValue = "Assigned";
            Assert.That(setting.StringValue, Is.EqualTo("Assigned"));
        }

        [Test]
        // HINT,Inder: Für StringValue, DecimalValue und IntegerValue eigene Tests machen...
        public void Set_Boolean_sets_String_and_Decimal_to_null()
        {
            var setting = new Setting();
            setting.StringValue = "Assigned";
            setting.DecimalValue = 1.1;
            setting.BooleanValue = true;
            Assert.That(setting.StringValue, Is.Null);
            Assert.That(setting.DecimalValue, Is.Null);
        }

        [Test]
        // HINT,Inder: Für StringValue, IntegerValue und BooleanValue eigene Tests machen...
        public void Set_Decimal_sets_String_and_Integer_to_null()
        {
            var setting = new Setting();
            setting.StringValue = "Assigned";
            setting.IntegerValue = 1;
            setting.DecimalValue = 1.1;
            Assert.That(setting.StringValue, Is.Null);
            Assert.That(setting.IntegerValue, Is.Null);
        }

        [Test]
        // HINT,Inder: Für StringValue, DecimalValue und BooleanValue eigene Tests machen...
        public void Set_Integer_sets_String_and_Decimal_to_null()
        {
            var setting = new Setting();
            setting.StringValue = "Assigned";
            setting.DecimalValue = 1.1;
            setting.IntegerValue = 1;
            Assert.That(setting.StringValue, Is.Null);
            Assert.That(setting.DecimalValue, Is.Null);
        }

        [Test]
        // HINT,Inder: Für DecimalValue, IntegerValue und BooleanValue eigene Tests machen...
        public void Set_String_sets_Decimal_and_Integer_to_null()
        {
            var setting = new Setting();
            setting.DecimalValue = 1.1;
            setting.IntegerValue = 1;
            setting.StringValue = "Assigned";
            Assert.That(setting.DecimalValue, Is.Null);
            Assert.That(setting.IntegerValue, Is.Null);
        }
    }
}
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities
{
    [TestFixture]
    internal class SettingTests
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
            Assert.That(setting.Boolean, Is.Null);
            setting.Boolean = true;
            Assert.That(setting.Boolean, Is.True);
        }

        [Test]
        public void Can_set_and_get_Decimal()
        {
            var setting = new Setting();
            Assert.That(setting.Decimal, Is.Null);
            setting.Decimal = 1.1d;
            Assert.That(setting.Decimal, Is.EqualTo(1.1d));
        }

        [Test]
        public void Can_set_and_get_Integer()
        {
            var setting = new Setting();
            Assert.That(setting.Integer, Is.Null);
            setting.Integer = 1;
            Assert.That(setting.Integer, Is.EqualTo(1));
        }

        [Test]
        public void Can_set_and_get_String()
        {
            var setting = new Setting();
            Assert.That(setting.String, Is.Null);
            setting.String = "Assigned";
            Assert.That(setting.String, Is.EqualTo("Assigned"));
        }

        [Test]
        // HINT,Inder: Für String, Decimal und Integer eigene Tests machen...
        public void Set_Boolean_sets_String_and_Decimal_to_null()
        {
            var setting = new Setting();
            setting.String = "Assigned";
            setting.Decimal = 1.1;
            setting.Boolean = true;
            Assert.That(setting.String, Is.Null);
            Assert.That(setting.Decimal, Is.Null);
        }

        [Test]
        // HINT,Inder: Für String, Integer und Boolean eigene Tests machen...
        public void Set_Decimal_sets_String_and_Integer_to_null()
        {
            var setting = new Setting();
            setting.String = "Assigned";
            setting.Integer = 1;
            setting.Decimal = 1.1;
            Assert.That(setting.String, Is.Null);
            Assert.That(setting.Integer, Is.Null);
        }

        [Test]
        // HINT,Inder: Für String, Decimal und Boolean eigene Tests machen...
        public void Set_Integer_sets_String_and_Decimal_to_null()
        {
            var setting = new Setting();
            setting.String = "Assigned";
            setting.Decimal = 1.1;
            setting.Integer = 1;
            Assert.That(setting.String, Is.Null);
            Assert.That(setting.Decimal, Is.Null);
        }

        [Test]
        // HINT,Inder: Für Decimal, Integer und Boolean eigene Tests machen...
        public void Set_String_sets_Decimal_and_Integer_to_null()
        {
            var setting = new Setting();
            setting.Decimal = 1.1;
            setting.Integer = 1;
            setting.String = "Assigned";
            Assert.That(setting.Decimal, Is.Null);
            Assert.That(setting.Integer, Is.Null);
        }
    }
}
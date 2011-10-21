using System;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities
{
    [TestFixture]
    public class DomainPropertyTypeTests
    {
        private static void AssertValue(FieldType actual, int expected)
        {
            Assert.That(Convert.ToInt32(actual), Is.EqualTo(expected));
        }

        [Test]
        public void Boolean_is_0()
        {
            AssertValue(FieldType.Boolean, 0);
        }

        [Test]
        public void DateTime_is_4()
        {
            AssertValue(FieldType.DateTime, 4);
        }

        [Test]
        public void Decimal_is_3()
        {
            AssertValue(FieldType.Decimal, 3);
        }

        [Test]
        public void Integer_is_2()
        {
            AssertValue(FieldType.Integer, 2);
        }

        [Test]
        public void Text_is_1()
        {
            AssertValue(FieldType.Text, 1);
        }
    }
}
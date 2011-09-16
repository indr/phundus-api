using System;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities
{
    [TestFixture]
    public class ItemPropertyTypeTests
    {
        private static void AssertValue(ItemPropertyType actual, int expected)
        {
            Assert.That(Convert.ToInt32(actual), Is.EqualTo(expected));
        }

        [Test]
        public void Boolean_is_0()
        {
            AssertValue(ItemPropertyType.Boolean, 0);
        }

        [Test]
        public void DateTime_is_4()
        {
            AssertValue(ItemPropertyType.DateTime, 4);
        }

        [Test]
        public void Decimal_is_3()
        {
            AssertValue(ItemPropertyType.Decimal, 3);
        }

        [Test]
        public void Integer_is_2()
        {
            AssertValue(ItemPropertyType.Integer, 2);
        }

        [Test]
        public void Text_is_1()
        {
            AssertValue(ItemPropertyType.Text, 1);
        }
    }
}
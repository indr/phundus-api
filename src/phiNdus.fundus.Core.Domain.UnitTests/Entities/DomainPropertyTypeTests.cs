using System;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities
{
    [TestFixture]
    public class DomainPropertyTypeTests
    {
        private static void AssertValue(DomainPropertyType actual, int expected)
        {
            Assert.That(Convert.ToInt32(actual), Is.EqualTo(expected));
        }

        [Test]
        public void Boolean_is_0()
        {
            AssertValue(DomainPropertyType.Boolean, 0);
        }

        [Test]
        public void DateTime_is_4()
        {
            AssertValue(DomainPropertyType.DateTime, 4);
        }

        [Test]
        public void Decimal_is_3()
        {
            AssertValue(DomainPropertyType.Decimal, 3);
        }

        [Test]
        public void Integer_is_2()
        {
            AssertValue(DomainPropertyType.Integer, 2);
        }

        [Test]
        public void Text_is_1()
        {
            AssertValue(DomainPropertyType.Text, 1);
        }
    }
}
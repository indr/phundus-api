using System;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities
{
    [TestFixture]
    public class DataTypeTests
    {
        private static void AssertValue(DataType actual, int expected)
        {
            Assert.That(Convert.ToInt32(actual), Is.EqualTo(expected));
        }

        [Test]
        public void Boolean_is_0()
        {
            AssertValue(DataType.Boolean, 0);
        }

        [Test]
        public void DateTime_is_4()
        {
            AssertValue(DataType.DateTime, 4);
        }

        [Test]
        public void Decimal_is_3()
        {
            AssertValue(DataType.Decimal, 3);
        }

        [Test]
        public void Integer_is_2()
        {
            AssertValue(DataType.Integer, 2);
        }

        [Test]
        public void Text_is_1()
        {
            AssertValue(DataType.Text, 1);
        }
    }
}
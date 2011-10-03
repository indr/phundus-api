using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities
{
    [TestFixture]
    public class OrderStatusTests
    {
        [Test]
        public void Pending_is_0()
        {
            Assert.That((int)OrderStatus.Pending, Is.EqualTo(0));
        }

        [Test]
        public void Approved_is_1()
        {
            Assert.That((int)OrderStatus.Approved, Is.EqualTo(1));
        }

        [Test]
        public void Rejected_is_2()
        {
            Assert.That((int)OrderStatus.Rejected, Is.EqualTo(2));
        }
    }
}
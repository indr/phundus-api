namespace Phundus.Core.Tests._Legacy.Entities
{
    using NUnit.Framework;
    using Phundus.Shop.Orders.Model;

    [TestFixture]
    public class OrderStatusTests
    {
        [Test]
        public void Approved_is_2()
        {
            Assert.That((int) OrderStatus.Approved, Is.EqualTo(2));
        }

        [Test]
        public void Pending_is_1()
        {
            Assert.That((int) OrderStatus.Pending, Is.EqualTo(1));
        }

        [Test]
        public void Rejected_is_3()
        {
            Assert.That((int) OrderStatus.Rejected, Is.EqualTo(3));
        }
    }
}
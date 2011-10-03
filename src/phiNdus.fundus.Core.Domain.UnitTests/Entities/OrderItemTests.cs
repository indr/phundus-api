using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities
{
    [TestFixture]
    public class OrderItemTests
    {
        private static OrderItem CreateSut()
        {
            return new OrderItem();
        }

        [Test]
        public void Can_get_and_set_Order()
        {
            var sut = CreateSut();
            var order = new Order();
            sut.Order = order;
            Assert.That(sut.Order, Is.SameAs(order));
        }

        [Test]
        public void Create_sets_Id_and_Version_to_0()
        {
            var sut = new OrderItem();
            Assert.That(sut.Id, Is.EqualTo(0));
            Assert.That(sut.Version, Is.EqualTo(0));
        }
    }
}
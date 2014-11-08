namespace Phundus.Core.Tests._Legacy.Entities
{
    using System;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Shop.Contracts.Model;
    using Core.Shop.Orders.Model;
    using NUnit.Framework;

    [TestFixture]
    public class OrderItemTests
    {
        private static OrderItem CreateSut()
        {
            return new OrderItem( new Order(new Organization(1001, "Organisation"), new Borrower(1, "", "", "", "", "","","","")),
                new Article(1, "Dummy"), DateTime.UtcNow, DateTime.UtcNow.AddDays(1), 1);
        }

        [Test]
        public void Can_get_and_set_Amount()
        {
            OrderItem sut = CreateSut();
            sut.Amount = 1;
            Assert.That(sut.Amount, Is.EqualTo(1));
        }

        [Test]
        public void Can_delete()
        {
            OrderItem sut = CreateSut();
            Assert.That(sut.Order, Is.Not.Null);
            sut.Delete();
            Assert.That(sut.Order, Is.Null);
        }

        [Test]
        public void Set_Amount_less_than_1_throws()
        {
            OrderItem sut = CreateSut();
            Assert.Throws<ArgumentOutOfRangeException>(() => sut.Amount = 0);
        }
    }
}
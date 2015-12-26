namespace Phundus.Core.Tests._Legacy.Entities
{
    using System;
    using Core.Inventory.Articles.Model;
    using Core.Shop.Contracts.Model;
    using Core.Shop.Orders.Model;
    using NUnit.Framework;
    using Article = Core.Inventory.Articles.Model.Article;

    [TestFixture]
    public class OrderItemTests
    {
        private static OrderItem CreateSut()
        {
            return new OrderItem( new Order(new Organization(1001, "Organisation"), new Borrower(1, "", "", "", "", "","","","")),
                new Article(1, "Dummy"), DateTime.UtcNow, DateTime.UtcNow.AddDays(1), 1);
        }

        [Test]
        public void Can_delete()
        {
            OrderItem sut = CreateSut();
            Assert.That(sut.Order, Is.Not.Null);
            sut.Delete();
            Assert.That(sut.Order, Is.Null);
        }
    }
}
namespace phiNdus.fundus.Domain.UnitTests.Entities
{
    using System;
    using NUnit.Framework;
    using Phundus.Core.Inventory.Articles.Model;
    using Phundus.Core.Shop.Contracts.Model;
    using Phundus.Core.Shop.Orders.Model;

    [TestFixture]
    public class OrderItemTests
    {
        private static OrderItem CreateSut()
        {
            return new OrderItem( new Order(new Organization(1001, "Organisation"), new Borrower(1, "", "", "", "", "","","","")),
                DateTime.UtcNow, DateTime.UtcNow.AddDays(1));
        }

        [Test]
        public void Can_get_and_set_Amount()
        {
            OrderItem sut = CreateSut();
            sut.Amount = 1;
            Assert.That(sut.Amount, Is.EqualTo(1));
        }

        [Test]
        public void Can_get_and_set_Article()
        {
            var article = new Article(1, "Dummy");
            OrderItem sut = CreateSut();
            sut.Article = article;
            Assert.That(sut.Article, Is.SameAs(article));
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
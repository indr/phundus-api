namespace phiNdus.fundus.Domain.UnitTests.Entities
{
    using System;
    using NUnit.Framework;
    using Phundus.Core.Inventory.Model;
    using Phundus.Core.Shop.Contracts.Model;
    using Phundus.Core.Shop.Orders.Model;

    [TestFixture]
    public class OrderItemTests
    {
        private static OrderItem CreateSut()
        {
            return new OrderItem();
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
        public void Can_get_and_set_From()
        {
            OrderItem sut = CreateSut();
            sut.From = DateTime.Today.AddDays(-1);
            Assert.That(sut.From, Is.EqualTo(DateTime.Today.AddDays(-1)));
        }

        [Test]
        public void Can_get_and_set_Order()
        {
            OrderItem sut = CreateSut();
            var order = new Order(1001, new Borrower(1, "", "", "", "", "","","",""));
            sut.Order = order;
            Assert.That(sut.Order, Is.SameAs(order));
        }

        [Test]
        public void Can_get_and_set_To()
        {
            OrderItem sut = CreateSut();
            sut.To = DateTime.Today.AddDays(1);
            Assert.That(sut.To, Is.EqualTo(DateTime.Today.AddDays(1)));
        }

        [Test]
        public void Set_Amount_less_than_1_throws()
        {
            OrderItem sut = CreateSut();
            Assert.Throws<ArgumentOutOfRangeException>(() => sut.Amount = 0);
        }
    }
}
namespace phiNdus.fundus.Domain.UnitTests.Entities
{
    using System;
    using NUnit.Framework;
    using Phundus.Core.InventoryCtx;
    using Phundus.Core.InventoryCtx.Model;
    using Phundus.Core.ReservationCtx;
    using Phundus.Core.ReservationCtx.Model;

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
            var article = new Article();
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
            var order = new Order();
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
        public void Create_sets_Id_and_Version_to_0()
        {
            var sut = new OrderItem();
            Assert.That(sut.Id, Is.EqualTo(0));
            Assert.That(sut.Version, Is.EqualTo(0));
        }

        [Test]
        public void Set_Amount_less_than_1_throws()
        {
            OrderItem sut = CreateSut();
            Assert.Throws<ArgumentOutOfRangeException>(() => sut.Amount = 0);
        }
    }
}
using System;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities
{
    [TestFixture]
    public class OrderTests
    {
        private static Order CreateSut()
        {
            return new Order();
        }

        [Test]
        public void AddItem_returns_true_and_adds_item_to_Items()
        {
            var sut = CreateSut();
            var item = new OrderItem();
            Assert.That(sut.AddItem(item), Is.True);
            Assert.That(sut.Items, Has.Some.SameAs(item));
        }

        [Test]
        public void AddItem_sets_Order_on_Item()
        {
            var sut = CreateSut();
            var item = new OrderItem();
            sut.AddItem(item);
            Assert.That(item.Order, Is.SameAs(sut));
        }

        [Test]
        public void AddItem_with_item_already_added_returns_false()
        {
            var sut = CreateSut();
            var item = new OrderItem();
            sut.AddItem(item);
            Assert.That(sut.AddItem(item), Is.False);
        }

        [Test]
        public void Create_assignes_empty_Items_collection()
        {
            var sut = new Order();
            Assert.That(sut.Items, Is.Not.Null);
            Assert.That(sut.Items, Has.Count.EqualTo(0));
        }

        [Test]
        public void Create_sets_CreateDate()
        {
            var sut = new Order();
            Assert.That(sut.CreateDate, Is.InRange(DateTime.Now.AddMinutes(-1), DateTime.Now.AddMinutes(1)));
        }

        [Test]
        public void Create_sets_Id_and_Version_to_0()
        {
            var sut = new Order();
            Assert.That(sut.Id, Is.EqualTo(0));
            Assert.That(sut.Version, Is.EqualTo(0));
        }

        [Test]
        public void RemoveItem_returns_true_and_removes_item_from_Items()
        {
            var sut = CreateSut();
            var item = new OrderItem();
            sut.AddItem(item);
            Assert.That(sut.RemoveItem(item), Is.True);
            Assert.That(sut.Items, Has.No.Some.SameAs(item));
        }

        [Test]
        public void RemoveItem_sets_Order_on_Item_to_null()
        {
            var sut = CreateSut();
            var item = new OrderItem();
            item.Order = sut;
            sut.RemoveItem(item);
            Assert.That(item.Order, Is.Null);
        }

        [Test]
        public void RemoveItem_without_item_added_returns_false()
        {
            var sut = CreateSut();
            var item = new OrderItem();
            Assert.That(sut.RemoveItem(item), Is.False);
        }
    }
}
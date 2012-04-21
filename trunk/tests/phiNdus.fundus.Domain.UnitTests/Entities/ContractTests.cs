using System;
using NUnit.Framework;
using phiNdus.fundus.Domain.Entities;

namespace phiNdus.fundus.Domain.UnitTests.Entities
{
    [TestFixture]
    public class ContractTests
    {
        private static Contract CreateSut()
        {
            return new Contract();
        }

        [Test]
        public void AddItem_returns_true_and_adds_item_to_Items()
        {
            var sut = CreateSut();
            var item = new ContractItem();
            Assert.That(sut.AddItem(item), Is.True);
            Assert.That(sut.Items, Has.Some.SameAs(item));
        }

        [Test]
        public void AddItem_sets_Contract_on_Item()
        {
            var sut = CreateSut();
            var item = new ContractItem();
            sut.AddItem(item);
            Assert.That(item.Contract, Is.SameAs(sut));
        }

        [Test]
        public void AddItem_with_item_already_added_returns_false()
        {
            var sut = CreateSut();
            var item = new ContractItem();
            sut.AddItem(item);
            Assert.That(sut.AddItem(item), Is.False);
        }

        [Test]
        public void Can_create()
        {
            var sut = new Contract();
            Assert.That(sut, Is.Not.Null);
        }

        [Test]
        public void Can_create_with_Id_and_Version()
        {
            var sut = new Contract(1, 2);
            Assert.That(sut.Id, Is.EqualTo(1));
            Assert.That(sut.Version, Is.EqualTo(2));
        }

        [Test]
        public void Can_get_and_set_Borrower()
        {
            var borrower = new User();
            var sut = new Contract();
            sut.Borrower = borrower;
            Assert.That(sut.Borrower, Is.SameAs(borrower));
        }

        [Test]
        public void Create_sets_CreateDate()
        {
            var sut = new Contract();
            Assert.That(sut.CreateDate, Is.InRange(DateTime.Now.AddMinutes(-1), DateTime.Now.AddMinutes(1)));
        }

        [Test]
        public void Create_sets_Id_and_Version_to_0()
        {
            var sut = new Contract();
            Assert.That(sut.Id, Is.EqualTo(0));
            Assert.That(sut.Version, Is.EqualTo(0));
        }

        [Test]
        public void RemoveItem_returns_true_and_removes_item_from_Items()
        {
            var sut = CreateSut();
            var item = new ContractItem();
            sut.AddItem(item);
            Assert.That(sut.RemoveItem(item), Is.True);
            Assert.That(sut.Items, Has.No.Some.SameAs(item));
        }

        [Test]
        public void RemoveItem_sets_Order_on_Item_to_null()
        {
            var sut = CreateSut();
            var item = new ContractItem();
            item.Contract = sut;
            sut.RemoveItem(item);
            Assert.That(item.Contract, Is.Null);
        }

        [Test]
        public void RemoveItem_without_item_added_returns_false()
        {
            var sut = CreateSut();
            var item = new ContractItem();
            Assert.That(sut.RemoveItem(item), Is.False);
        }

        [Test]
        public void Can_get_and_set_From()
        {
            var sut = CreateSut();
            sut.From = DateTime.Today;
            Assert.That(sut.From, Is.EqualTo(DateTime.Today));
        }

        [Test]
        public void Can_get_and_set_To()
        {
            var sut = CreateSut();
            sut.To = DateTime.Today;
            Assert.That(sut.To, Is.EqualTo(DateTime.Today));
        }

        [Test]
        public void Can_get_and_set_Order()
        {
            var sut = CreateSut();
            var order = new Order();
            sut.Order = order;
            Assert.That(sut.Order, Is.SameAs(order));
        }
    }
}
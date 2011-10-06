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
        public void Can_create_with_Id_and_Version()
        {
            var sut = new Order(1, 2);
            Assert.That(sut.Id, Is.EqualTo(1));
            Assert.That(sut.Version, Is.EqualTo(2));
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
        public void Can_get_and_set_Reserver()
        {
            var sut = CreateSut();
            var reserver = new User();
            sut.Reserver = reserver;
            Assert.That(sut.Reserver, Is.SameAs(reserver));
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
        public void Create_sets_Status_to_Pending()
        {
            var sut = new Order();
            Assert.That(sut.Status, Is.EqualTo(OrderStatus.Pending));
        }

        [Test]
        public void Create_sets_Modifier_to_null()
        {
            var sut = new Order();
            Assert.That(sut.Modifier, Is.Null);
        }

        [Test]
        public void Create_sets_ModifyDate_to_null()
        {
            var sut = new Order();
            Assert.That(sut.ModifyDate, Is.Null);
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

        [Test]
        public void Approve_with_null_approver_throws()
        {
            var sut = CreateSut();
            var ex = Assert.Throws<ArgumentNullException>(() => sut.Approve(null));
            Assert.That(ex.ParamName, Is.EqualTo("approver"));
        }

        [Test]
        public void Approve_sets_Mofidier_to_supplied_user()
        {
            var sut = CreateSut();
            var approver = new User();
            sut.Approve(approver);
            Assert.That(sut.Modifier, Is.SameAs(approver));
        }

        [Test]
        public void Approve_sets_ModifyDate()
        {
            var sut = CreateSut();
            sut.Approve(new User());
            Assert.That(sut.ModifyDate, Is.InRange(DateTime.Now.AddMinutes(-1), DateTime.Now.AddMinutes(1)));
        }

        [Test]
        public void Approve_sets_Status_to_Approved()
        {
            var sut = CreateSut();
            sut.Approve(new User());
            Assert.That(sut.Status, Is.EqualTo(OrderStatus.Approved));
        }

        [Test]
        public void Approve_with_status_Approved_throws()
        {
            var sut = CreateSut();
            sut.Approve(new User());
            var ex = Assert.Throws<InvalidOperationException>(() => sut.Approve(new User()));
            Assert.That(ex.Message, Is.EqualTo("Die Bestellung wurde bereits bewilligt."));
        }

        [Test]
        public void Approve_with_status_Rejected_throws()
        {
            var sut = CreateSut();
            sut.Reject(new User());
            var ex = Assert.Throws<InvalidOperationException>(() => sut.Approve(new User()));
            Assert.That(ex.Message, Is.EqualTo("Die Bestellung wurde bereits abgelehnt."));
        }

        [Test]
        public void Reject_with_null_rejecter_throws()
        {
            var sut = CreateSut();
            var ex = Assert.Throws<ArgumentNullException>(() => sut.Reject(null));
            Assert.That(ex.ParamName, Is.EqualTo("rejecter"));
        }

        [Test]
        public void Reject_sets_Modifier_to_supplied_user()
        {
            var sut = CreateSut();
            var rejecter = new User();
            sut.Reject(rejecter);
            Assert.That(sut.Modifier, Is.SameAs(rejecter));
        }

        [Test]
        public void Reject_sets_ModifyDate()
        {
            var sut = CreateSut();
            sut.Reject(new User());
            Assert.That(sut.ModifyDate, Is.InRange(DateTime.Now.AddMinutes(-1), DateTime.Now.AddMinutes(1)));
        }

        [Test]
        public void Reject_sets_Status_to_Rejected()
        {
            var sut = CreateSut();
            sut.Reject(new User());
            Assert.That(sut.Status, Is.EqualTo(OrderStatus.Rejected));
        }

        [Test]
        public void Reject_with_status_Approved_throws()
        {
            var sut = CreateSut();
            sut.Approve(new User());
            var ex = Assert.Throws<InvalidOperationException>(() => sut.Reject(new User()));
            Assert.That(ex.Message, Is.EqualTo("Die Bestellung wurde bereits bewilligt."));
        }

        [Test]
        public void Reject_with_status_Rejected_throws()
        {
            var sut = CreateSut();
            sut.Reject(new User());
            var ex = Assert.Throws<InvalidOperationException>(() => sut.Reject(new User()));
            Assert.That(ex.Message, Is.EqualTo("Die Bestellung wurde bereits abgelehnt."));
        }
    }
}
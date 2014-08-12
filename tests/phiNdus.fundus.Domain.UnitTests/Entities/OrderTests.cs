namespace phiNdus.fundus.Domain.UnitTests.Entities
{
    using System;
    using NUnit.Framework;
    using Phundus.Core.IdentityAndAccess.Users.Model;
    using Phundus.Core.ReservationCtx;
    using Phundus.Core.ReservationCtx.Model;
    using Phundus.Core.Shop.Contracts.Model;
    using Phundus.Core.Shop.Orders.Model;

    [TestFixture]
    public class OrderTests
    {
        private static Order CreateSut()
        {
            return new Order(1001, new Borrower(1, "", "", "", "", "", "", "", ""));
        }

        [Test]
        public void Approve_sets_ModifyDate()
        {
            Order sut = CreateSut();
            sut.Approve(new User());
            Assert.That(sut.ModifyDate, Is.InRange(DateTime.Now.AddMinutes(-1), DateTime.Now.AddMinutes(1)));
        }

        [Test]
        public void Approve_sets_Mofidier_to_supplied_user()
        {
            Order sut = CreateSut();
            var approver = new User();
            sut.Approve(approver);
            Assert.That(sut.Modifier, Is.SameAs(approver));
        }

        [Test]
        public void Approve_sets_Status_to_Approved()
        {
            Order sut = CreateSut();
            sut.Approve(new User());
            Assert.That(sut.Status, Is.EqualTo(OrderStatus.Approved));
        }

        [Test]
        public void Approve_with_null_approver_throws()
        {
            Order sut = CreateSut();
            var ex = Assert.Throws<ArgumentNullException>(() => sut.Approve(null));
            Assert.That(ex.ParamName, Is.EqualTo("approver"));
        }

        [Test]
        public void Approve_with_status_Approved_throws()
        {
            Order sut = CreateSut();
            sut.Approve(new User());
            var ex = Assert.Throws<InvalidOperationException>(() => sut.Approve(new User()));
            Assert.That(ex.Message, Is.EqualTo("Die Bestellung wurde bereits bewilligt."));
        }

        [Test]
        public void Approve_with_status_Rejected_throws()
        {
            Order sut = CreateSut();
            sut.Reject(new User());
            var ex = Assert.Throws<InvalidOperationException>(() => sut.Approve(new User()));
            Assert.That(ex.Message, Is.EqualTo("Die Bestellung wurde bereits abgelehnt."));
        }

        [Test]
        public void Create_assignes_empty_Items_collection()
        {
            var sut = CreateSut();
            Assert.That(sut.Items, Is.Not.Null);
            Assert.That(sut.Items, Has.Count.EqualTo(0));
        }

        [Test]
        public void Create_sets_CreateDate()
        {
            var sut = CreateSut();
            Assert.That(sut.CreatedOn, Is.InRange(DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow.AddMinutes(1)));
        }

        [Test]
        public void Create_sets_Id_and_Version_to_0()
        {
            var sut = CreateSut();
            Assert.That(sut.Id, Is.EqualTo(0));
            Assert.That(sut.Version, Is.EqualTo(0));
        }

        [Test]
        public void Create_sets_Modifier_to_null()
        {
            var sut = CreateSut();
            Assert.That(sut.Modifier, Is.Null);
        }

        [Test]
        public void Create_sets_ModifyDate_to_null()
        {
            var sut = CreateSut();
            Assert.That(sut.ModifyDate, Is.Null);
        }

        [Test]
        public void Create_sets_Status_to_Pending()
        {
            var sut = CreateSut();
            Assert.That(sut.Status, Is.EqualTo(OrderStatus.Pending));
        }

        [Test]
        public void GetTotalPrice()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            decimal totalPrice = sut.TotalPrice;

            // Assert
            Assert.That(totalPrice, Is.EqualTo(0.0));
        }

        [Test]
        public void Reject_sets_Modifier_to_supplied_user()
        {
            Order sut = CreateSut();
            var rejecter = new User();
            sut.Reject(rejecter);
            Assert.That(sut.Modifier, Is.SameAs(rejecter));
        }

        [Test]
        public void Reject_sets_ModifyDate()
        {
            Order sut = CreateSut();
            sut.Reject(new User());
            Assert.That(sut.ModifyDate, Is.InRange(DateTime.Now.AddMinutes(-1), DateTime.Now.AddMinutes(1)));
        }

        [Test]
        public void Reject_sets_Status_to_Rejected()
        {
            Order sut = CreateSut();
            sut.Reject(new User());
            Assert.That(sut.Status, Is.EqualTo(OrderStatus.Rejected));
        }

        [Test]
        public void Reject_with_null_rejecter_throws()
        {
            Order sut = CreateSut();
            var ex = Assert.Throws<ArgumentNullException>(() => sut.Reject(null));
            Assert.That(ex.ParamName, Is.EqualTo("rejecter"));
        }

        [Test]
        public void Reject_with_status_Approved_throws()
        {
            Order sut = CreateSut();
            sut.Approve(new User());
            var ex = Assert.Throws<InvalidOperationException>(() => sut.Reject(new User()));
            Assert.That(ex.Message, Is.EqualTo("Die Bestellung wurde bereits bewilligt."));
        }

        [Test]
        public void Reject_with_status_Rejected_throws()
        {
            Order sut = CreateSut();
            sut.Reject(new User());
            var ex = Assert.Throws<InvalidOperationException>(() => sut.Reject(new User()));
            Assert.That(ex.Message, Is.EqualTo("Die Bestellung wurde bereits abgelehnt."));
        }
    }
}
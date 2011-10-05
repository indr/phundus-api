using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Assembler;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Business.UnitTests.Assembler
{
    [TestFixture]
    public class OrderAssemblerTests : BaseTestFixture
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            
            
            Reserver = new User(1);
            Reserver.FirstName = "Robert";
            Reserver.LastName = "Reservierer";

            Approver = new User(2);
            Approver.FirstName = "Berta";
            Approver.LastName = "Bestätigerin";

            Rejecter = new User(3);
            Rejecter.FirstName = "Albert";
            Rejecter.LastName = "Ablehner";
        }

        protected User Reserver { get; set; }
        protected User Approver { get; set; }
        protected User Rejecter { get; set; }

        private Order CreateOrder()
        {
            var result = new Order(1, 2);
            result.Reserver = Reserver;
            return result;
        }

        [Test]
        public void CreateDto_returns_dto()
        {
            var domain = CreateOrder();
            var dto = OrderAssembler.CreateDto(domain);

            Assert.That(dto, Is.Not.Null);
            Assert.That(dto.Id, Is.EqualTo(1));
            Assert.That(dto.Version, Is.EqualTo(2));
            Assert.That(dto.CreateDate, Is.InRange(DateTime.Now.AddHours(-1), DateTime.Now.AddHours(1)));
            Assert.That(dto.ReserverId, Is.EqualTo(Reserver.Id));
            Assert.That(dto.ReserverName, Is.EqualTo(Reserver.DisplayName));
            Assert.That(dto.ApproveDate, Is.Null);
            Assert.That(dto.ApproverId, Is.Null);
            Assert.That(dto.ApproverName, Is.Null);
            Assert.That(dto.RejectDate, Is.Null);
            Assert.That(dto.RejecterId, Is.Null);
            Assert.That(dto.RejecterName, Is.Null);
        }

        [Test]
        public void CreateDto_with_subject_approved_returns_dto()
        {
            var domain = CreateOrder();
            domain.Approve(Approver);
            var dto = OrderAssembler.CreateDto(domain);
            Assert.That(dto.ApproveDate, Is.EqualTo(domain.ApproveDate));
            Assert.That(dto.ApproverId, Is.EqualTo(domain.Approver.Id));
            Assert.That(dto.ApproverName, Is.EqualTo(domain.Approver.DisplayName));
        }

        [Test]
        public void CreateDto_with_subject_rejected_returns_dto()
        {
            var domain = CreateOrder();
            domain.Reject(Rejecter);
            var dto = OrderAssembler.CreateDto(domain);
            Assert.That(dto.RejectDate, Is.EqualTo(domain.RejectDate));
            Assert.That(dto.RejecterId, Is.EqualTo(domain.Rejecter.Id));
            Assert.That(dto.RejecterName, Is.EqualTo(domain.Rejecter.DisplayName));
        }

        [Test]
        public void CreateDto_with_subject_null_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => OrderAssembler.CreateDto(null));
            Assert.That(ex.ParamName, Is.EqualTo("subject"));
        }
    }
}

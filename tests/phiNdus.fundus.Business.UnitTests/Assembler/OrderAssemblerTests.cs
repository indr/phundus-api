using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using phiNdus.fundus.Business.Assembler;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.TestHelpers.TestBases;

namespace phiNdus.fundus.Business.UnitTests.Assembler
{
    [TestFixture]
    public class OrderAssemblerTests : UnitTestBase<object>
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
            var dto = new OrderDtoAssembler().CreateDto(domain);

            Assert.That(dto, Is.Not.Null);
            Assert.That(dto.Id, Is.EqualTo(1));
            Assert.That(dto.Version, Is.EqualTo(2));
            Assert.That(dto.CreateDate, Is.InRange(DateTime.Now.AddHours(-1), DateTime.Now.AddHours(1)));
            Assert.That(dto.ReserverId, Is.EqualTo(Reserver.Id));
            Assert.That(dto.ReserverName, Is.EqualTo(Reserver.DisplayName));
            Assert.That(dto.ModifyDate, Is.Null);
            Assert.That(dto.ModifierId, Is.Null);
            Assert.That(dto.ModifierName, Is.Null);
        }

        [Test]
        public void CreateDto_with_subject_approved_returns_dto()
        {
            var domain = CreateOrder();
            domain.Approve(Approver);
            var dto = new OrderDtoAssembler().CreateDto(domain);
            Assert.That(dto.ModifyDate, Is.EqualTo(domain.ModifyDate));
            Assert.That(dto.ModifierId, Is.EqualTo(domain.Modifier.Id));
            Assert.That(dto.ModifierName, Is.EqualTo(domain.Modifier.DisplayName));
        }

        [Test]
        public void CreateDto_with_subject_rejected_returns_dto()
        {
            var domain = CreateOrder();
            domain.Reject(Rejecter);
            var dto = new OrderDtoAssembler().CreateDto(domain);
            Assert.That(dto.ModifyDate, Is.EqualTo(domain.ModifyDate));
            Assert.That(dto.ModifierId, Is.EqualTo(domain.Modifier.Id));
            Assert.That(dto.ModifierName, Is.EqualTo(domain.Modifier.DisplayName));
        }

        [Test]
        public void CreateDto_with_subject_null_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new OrderDtoAssembler().CreateDto(null));
            Assert.That(ex.ParamName, Is.EqualTo("subject"));
        }

        [Test]
        public void CreateDtos_returns_dtos()
        {
            var orders = new List<Order>();
            orders.Add(CreateOrder());
            orders.Add(CreateOrder());

            var dtos = new OrderDtoAssembler().CreateDtos(orders);

            Assert.That(dtos, Is.Not.Null);
            Assert.That(dtos, Has.Count.EqualTo(2));
        }

        [Test]
        public void CreateDtos_with_subjects_null_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new OrderDtoAssembler().CreateDtos(null));
            Assert.That(ex.ParamName, Is.EqualTo("subjects"));
        }
    }
}

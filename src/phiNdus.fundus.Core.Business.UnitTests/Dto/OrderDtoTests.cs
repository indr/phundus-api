using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Business.UnitTests.Dto
{
    [TestFixture]
    public class OrderDtoTests
    {
        [SetUp]
        public void SetUp()
        {
            Sut = new OrderDto();
        }

        protected OrderDto Sut { get; set; }

        [Test]
        public void Create_assignes_Items()
        {
            var sut = new OrderDto();
            Assert.That(sut.Items, Is.Not.Null);
            Assert.That(sut.Items, Has.Count.EqualTo(0));
        }

        [Test]
        public void Create_sets_ApproveDate_to_null()
        {
            var sut = new OrderDto();
            Assert.That(sut.ApproveDate, Is.Null);
        }

        [Test]
        public void Create_sets_ApproverId_to_null()
        {
            var sut = new OrderDto();
            Assert.That(sut.ApproverId, Is.Null);
        }

        [Test]
        public void Create_sets_ApproverName_to_null()
        {
            var sut = new OrderDto();
            Assert.That(sut.ApproverName, Is.Null);
        }

        [Test]
        public void Create_sets_RejectDate_to_null()
        {
            var sut = new OrderDto();
            Assert.That(sut.RejectDate, Is.Null);
        }

        [Test]
        public void Create_sets_RejecterId_to_null()
        {
            var sut = new OrderDto();
            Assert.That(sut.RejecterId, Is.Null);
        }

        [Test]
        public void Create_sets_RejecterName_to_null()
        {
            var sut = new OrderDto();
            Assert.That(sut.RejecterName, Is.Null);
        }

        [Test]
        public void Can_get_and_set_Id()
        {
            Sut.Id = 1;
            Assert.That(Sut.Id, Is.EqualTo(1));
        }

        [Test]
        public void Can_get_and_set_Version()
        {
            Sut.Version = 2;
            Assert.That(Sut.Version, Is.EqualTo(2));
        }

        [Test]
        public void Can_get_and_set_CreateDate()
        {
            Sut.CreateDate = DateTime.Today;
            Assert.That(Sut.CreateDate, Is.EqualTo(DateTime.Today));
        }

        [Test]
        public void Can_get_and_set_ReserverId()
        {
            Sut.ReserverId = 1;
            Assert.That(Sut.ReserverId, Is.EqualTo(1));
        }
        
        [Test]
        public void Can_get_and_set_ReserverName()
        {
            Sut.ReserverName = "Hans";
            Assert.That(Sut.ReserverName, Is.EqualTo("Hans"));
        }

        [Test]
        public void Can_get_and_set_ApproveDate()
        {
            Sut.ApproveDate = DateTime.Today;
            Assert.That(Sut.ApproveDate, Is.EqualTo(DateTime.Today));
        }

        [Test]
        public void Can_get_and_set_ApproverId()
        {
            Sut.ApproverId = 1;
            Assert.That(Sut.ApproverId, Is.EqualTo(1));
        }

        [Test]
        public void Can_get_and_set_ApproverName()
        {
            Sut.ApproverName = "Jakobli";
            Assert.That(Sut.ApproverName, Is.EqualTo("Jakobli"));
        }

        [Test]
        public void Can_get_and_set_RejectDate()
        {
            Sut.RejectDate = DateTime.Today;
            Assert.That(Sut.RejectDate, Is.EqualTo(DateTime.Today));
        }

        [Test]
        public void Can_get_and_set_RejecterId()
        {
            Sut.RejecterId = 1;
            Assert.That(Sut.RejecterId, Is.EqualTo(1));
        }

        [Test]
        public void Can_get_and_set_RejecterName()
        {
            Sut.RejecterName = "Babettli";
            Assert.That(Sut.RejecterName, Is.EqualTo("Babettli"));
        }
    }
}

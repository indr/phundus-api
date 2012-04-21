using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using phiNdus.fundus.Business.Dto;

namespace phiNdus.fundus.Business.UnitTests.Dto
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
        public void Create_sets_ModifyDate_to_null()
        {
            var sut = new OrderDto();
            Assert.That(sut.ModifyDate, Is.Null);
        }

        [Test]
        public void Create_sets_ModifierId_to_null()
        {
            var sut = new OrderDto();
            Assert.That(sut.ModifierId, Is.Null);
        }

        [Test]
        public void Create_sets_ModifierName_to_null()
        {
            var sut = new OrderDto();
            Assert.That(sut.ModifierName, Is.Null);
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
        public void Can_get_and_set_ModifyDate()
        {
            Sut.ModifyDate = DateTime.Today;
            Assert.That(Sut.ModifyDate, Is.EqualTo(DateTime.Today));
        }

        [Test]
        public void Can_get_and_set_ModifierId()
        {
            Sut.ModifierId = 1;
            Assert.That(Sut.ModifierId, Is.EqualTo(1));
        }

        [Test]
        public void Can_get_and_set_ModifierName()
        {
            Sut.ModifierName = "Jakobli";
            Assert.That(Sut.ModifierName, Is.EqualTo("Jakobli"));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Business.UnitTests.Dto
{
    [TestFixture]
    public class OrderItemDtoTests
    {
        [SetUp]
        public void SetUp()
        {
            Sut = new OrderItemDto();
        }

        protected OrderItemDto Sut { get; set; }

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
        public void Can_get_and_set_ArticleId()
        {
            Sut.ArticleId = 3;
            Assert.That(Sut.ArticleId, Is.EqualTo(3));
        }

        [Test]
        public void Can_get_and_set_From()
        {
            Sut.From = DateTime.Today;
            Assert.That(Sut.From, Is.EqualTo(Sut.From));
        }

        [Test]
        public void Can_get_and_set_To()
        {
            Sut.To = DateTime.Today;
            Assert.That(Sut.To, Is.EqualTo(DateTime.Today));
        }
    }
}

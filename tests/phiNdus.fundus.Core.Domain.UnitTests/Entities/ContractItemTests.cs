using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities
{
    [TestFixture]
    public class ContractItemTests
    {

        [Test]
        public void Can_create()
        {
            var sut = new ContractItem();
            Assert.That(sut, Is.Not.Null);
        }

        [Test]
        public void Can_create_with_Id_and_Version()
        {
            var sut = new ContractItem(1, 2);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Id, Is.EqualTo(1));
            Assert.That(sut.Version, Is.EqualTo(2));
        }

        [Test]
        public void Create_sets_ReturnDate_to_null()
        {
            var sut = new ContractItem();
            Assert.That(sut.ReturnDate, Is.Null);
        }

        [Test]
        public void Create_with_Id_and_Version_sets_ReturnDate_to_null()
        {
            var sut = new ContractItem(1, 2);
            Assert.That(sut.ReturnDate, Is.Null);
        }

        [Test]
        public void Can_get_and_set_Contract()
        {
            var contract = new Contract();
            var sut = new ContractItem();
            sut.Contract = contract;
            Assert.That(sut.Contract, Is.SameAs(contract));
        }

        [Test]
        public void Can_get_and_set_Amount()
        {
            var sut = new ContractItem();
            sut.Amount = 1;
            Assert.That(sut.Amount, Is.EqualTo(1));
        }

        [Test]
        public void Can_get_and_set_Article()
        {
            var article = new Article();
            var sut = new ContractItem();
            sut.Article = article;
            Assert.That(sut.Article, Is.SameAs(article));
        }

        [Test]
        public void Can_get_and_set_InventoryCode()
        {
            var sut = new ContractItem();
            sut.InventoryCode = "1234";
            Assert.That(sut.InventoryCode, Is.EqualTo("1234"));
        }

        [Test]
        public void Can_get_and_set_Name()
        {
            var sut = new ContractItem();
            sut.Name = "ThinkPad";
            Assert.That(sut.Name, Is.EqualTo("ThinkPad"));
        }

        [Test]
        public void Can_get_and_set_OrderItem()
        {
            var sut = new ContractItem();
            var orderItem = new OrderItem();
            sut.OrderItem = orderItem;
            Assert.That(sut.OrderItem, Is.SameAs(orderItem));
        }
    }
}

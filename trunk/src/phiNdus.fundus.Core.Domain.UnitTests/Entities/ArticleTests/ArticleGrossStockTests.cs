using System;
using Iesi.Collections.Generic;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.TestHelpers;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities.ArticleTests
{
    [TestFixture]
    public class ArticleGrossStockTests : ArticleTestBase
    {
        [Test]
        public void Get_with_children_and_attached_gross_stock_field_throws()
        {
            Sut.GrossStock = 100;
            AddChild();

            Assert.Throws<IllegalAttachedFieldException>(delegate { object current = Sut.GrossStock; });
        }

        [Test]
        public void Get_with_children_returns_sum_of_childrens_GrossStock()
        {
            var child1 = MockRepository.GenerateMock<Article>();
            var child2 = MockRepository.GenerateMock<Article>();
            Sut.AddChild(child1);
            Sut.AddChild(child2);

            child1.Expect(x => x.GrossStock).Return(10);
            child2.Expect(x => x.GrossStock).Return(20);

            var actual = Sut.GrossStock;
            Assert.That(actual, Is.EqualTo(30));
        }

        [Test]
        public void Get_without_children_and_attached_gross_stock_field_returns_field_value()
        {
            Sut.GrossStock = 100;

            var actual = Sut.GrossStock;
            Assert.That(actual, Is.EqualTo(100));
        }

        /// <summary>
        /// Kein definierter Bestand, keine Kindelemente => Bruttobestand = 1
        /// </summary>
        [Test]
        public void Get_without_children_and_without_attached_gross_stock_field_returns_1()
        {
            var actual = Sut.GrossStock;
            Assert.That(actual, Is.EqualTo(1));
        }

        [Test]
        public void Set_gross_stock_with_children_throws()
        {
            AddChild();
            Assert.Throws<InvalidOperationException>(() => Sut.GrossStock = 1);
        }

        [Test]
        public void Set_without_children()
        {
            Sut.GrossStock = 10;
            Assert.That(Sut.GrossStock, Is.EqualTo(10));
        }
    }
}
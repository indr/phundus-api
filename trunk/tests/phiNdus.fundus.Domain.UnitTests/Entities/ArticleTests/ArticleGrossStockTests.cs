namespace phiNdus.fundus.Domain.UnitTests.Entities.ArticleTests
{
    using System;
    using NUnit.Framework;
    using Phundus.Core.InventoryCtx;
    using Phundus.Core.InventoryCtx.Exceptions;
    using Phundus.Core.InventoryCtx.Model;
    using Rhino.Mocks;

    [TestFixture]
    public class ArticleGrossStockTests : ArticleTestBase
    {
        protected Article CreateSut()
        {
            return new Article();
        }

        [Test]
        public void Get_with_children_and_attached_gross_stock_field_throws()
        {
            Article sut = CreateSut();
            sut.GrossStock = 100;
            AddChild(sut);

            Assert.Throws<IllegalAttachedFieldException>(delegate { object value = sut.GrossStock; });
        }

        [Test]
        public void Get_with_children_returns_sum_of_childrens_GrossStock()
        {
            Article sut = CreateSut();
            var child1 = MockRepository.GenerateMock<Article>();
            var child2 = MockRepository.GenerateMock<Article>();
            sut.AddChild(child1);
            sut.AddChild(child2);

            child1.Expect(x => x.GrossStock).Return(10);
            child2.Expect(x => x.GrossStock).Return(20);

            int actual = sut.GrossStock;
            Assert.That(actual, Is.EqualTo(30));
        }

        [Test]
        public void Get_without_children_and_attached_gross_stock_field_returns_field_value()
        {
            Article sut = CreateSut();
            sut.GrossStock = 100;

            int actual = sut.GrossStock;
            Assert.That(actual, Is.EqualTo(100));
        }

        /// <summary>
        ///     Kein definierter Bestand, keine Kindelemente => Bruttobestand = 1
        /// </summary>
        [Test]
        public void Get_without_children_and_without_attached_gross_stock_field_returns_1()
        {
            Article sut = CreateSut();
            int actual = sut.GrossStock;
            Assert.That(actual, Is.EqualTo(1));
        }

        [Test]
        public void Set_gross_stock_with_children_throws()
        {
            Article sut = CreateSut();
            AddChild(sut);
            Assert.Throws<InvalidOperationException>(() => sut.GrossStock = 1);
        }

        [Test]
        public void Set_without_children()
        {
            Article sut = CreateSut();
            sut.GrossStock = 10;
            Assert.That(sut.GrossStock, Is.EqualTo(10));
        }
    }
}
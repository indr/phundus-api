namespace Phundus.Core.Tests._Legacy.Entities.ArticleTests
{
    using Core.Inventory.Domain.Model.Catalog;
    using NUnit.Framework;

    [TestFixture]
    public class ArticleGrossStockTests : ArticleTestBase
    {
        protected Article CreateSut()
        {
            return new Article(1, "Dummy");
        }

        [Test]
        public void Get_without_children_and_attached_gross_stock_field_returns_field_value()
        {
            Article sut = CreateSut();
            sut.GrossStock = 100;

            int actual = sut.GrossStock;
            Assert.That(actual, Is.EqualTo(100));
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
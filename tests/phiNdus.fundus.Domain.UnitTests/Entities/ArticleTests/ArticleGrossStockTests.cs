namespace phiNdus.fundus.Domain.UnitTests.Entities.ArticleTests
{
    using NUnit.Framework;
    using Phundus.Core.InventoryCtx.Model;

    [TestFixture]
    public class ArticleGrossStockTests : ArticleTestBase
    {
        protected Article CreateSut()
        {
            return new Article();
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
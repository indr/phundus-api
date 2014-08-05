namespace phiNdus.fundus.Domain.UnitTests.Entities.ArticleTests
{
    using NUnit.Framework;
    using Phundus.Core.Inventory.Model;
    using Rhino.Mocks;

    [TestFixture]
    public class ArticleBorrowableStockTests : ArticleTestBase
    {
        protected Article CreateSut()
        {
            return new Article(1, "Dummy");
        }

        
        [Test]
        public void Get_without_children()
        {
            // Typ-A, Massenwaren
            Article sut = CreateSut();
            SetAlreadyReservedAmount(sut.Id, 10);
            sut.GrossStock = 15;

            int actual = sut.ReservableStock;
            Assert.That(actual, Is.EqualTo(5));
        }
    }
}
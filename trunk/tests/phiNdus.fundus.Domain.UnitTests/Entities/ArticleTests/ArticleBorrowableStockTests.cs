namespace phiNdus.fundus.Domain.UnitTests.Entities.ArticleTests
{
    using NUnit.Framework;
    using Phundus.Core.InventoryCtx;
    using Phundus.Core.InventoryCtx.Model;
    using Rhino.Mocks;

    [TestFixture]
    public class ArticleBorrowableStockTests : ArticleTestBase
    {
        protected Article CreateSut()
        {
            return new Article();
        }

        [Test]
        public void Get_with_children_returns_sum_of_childrens_BorrowableStock()
        {
            // Typ-C, Mengenwaren
            Article sut = CreateSut();
            var child1 = MockRepository.GenerateMock<Article>();
            var child2 = MockRepository.GenerateMock<Article>();
            sut.AddChild(child1);
            sut.AddChild(child2);

            child1.Expect(x => x.ReservableStock).Return(10);
            child2.Expect(x => x.ReservableStock).Return(20);

            int actual = sut.ReservableStock;
            Assert.That(actual, Is.EqualTo(30));
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
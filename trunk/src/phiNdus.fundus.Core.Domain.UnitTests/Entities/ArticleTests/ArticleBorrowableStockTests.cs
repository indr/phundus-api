using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities.ArticleTests
{
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
            var sut = CreateSut();
            var child1 = MockRepository.GenerateMock<Article>();
            var child2 = MockRepository.GenerateMock<Article>();
            sut.AddChild(child1);
            sut.AddChild(child2);

            child1.Expect(x => x.ReservableStock).Return(10);
            child2.Expect(x => x.ReservableStock).Return(20);

            var actual = sut.ReservableStock;
            Assert.That(actual, Is.EqualTo(30));
        }

        [Test]
        public void Get_without_children()
        {
            // Typ-A, Massenwaren
            var sut = CreateSut();
            SetAlreadyReservedAmount(sut.Id, 10);
            sut.GrossStock = 15;

            var actual = sut.ReservableStock;
            Assert.That(actual, Is.EqualTo(5));
        }
    }
}
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities.ArticleTests
{
    [TestFixture]
    public class ArticleBorrowableStockTests : ArticleTestBase
    {
        [Test]
        public void Get_with_children_returns_sum_of_childrens_BorrowableStock()
        {
            // Typ-C, Mengenwaren
            var child1 = MockRepository.GenerateMock<Article>();
            var child2 = MockRepository.GenerateMock<Article>();
            Sut.AddChild(child1);
            Sut.AddChild(child2);

            child1.Expect(x => x.BorrowableStock).Return(10);
            child2.Expect(x => x.BorrowableStock).Return(20);

            var actual = Sut.BorrowableStock;
            Assert.That(actual, Is.EqualTo(30));
        }

        [Test]
        public void Get_without_children()
        {
            // Typ-A, Massenwaren
            SetAlreadyReservedAmount(Sut.Id, 10);
            Sut.GrossStock = 15;

            var actual = Sut.BorrowableStock;
            Assert.That(actual, Is.EqualTo(5));
        }
    }
}
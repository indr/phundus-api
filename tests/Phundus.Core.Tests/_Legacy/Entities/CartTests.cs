namespace Phundus.Core.Tests._Legacy.Entities
{
    using System;
    using Core.Inventory.Articles.Repositories;
    using Core.Shop.Orders.Model;
    using NUnit.Framework;

    [TestFixture]
    public class CartTests : UnitTestBase<Cart>
    {
        [Test]
        public void AreItemsAvailable_WithOneItemUnavailable_ReturnsFalse()
        {
            // Arrange
            var articleRepository = GenerateAndRegisterStub<IArticleRepository>();
            var sut = new Cart();
            sut.AddItem(new ArticleWrapper(articleRepository.GetById(1)), 1, DateTime.Today, DateTime.Today.AddDays(1));

            // Act
            bool actual = sut.AreItemsAvailable;

            // Assert
            Assert.That(actual, Is.False);
        }
    }
}
namespace phiNdus.fundus.Domain.UnitTests.Entities
{
    using System;
    using NUnit.Framework;
    using Phundus.Core.InventoryCtx;
    using Phundus.Core.InventoryCtx.Repositories;
    using Phundus.Core.ShopCtx;
    using TestHelpers.TestBases;

    [TestFixture]
    public class CartTests : UnitTestBase<Cart>
    {
        [Test]
        public void AreItemsAvailable_WithOneItemUnavailable_ReturnsFalse()
        {
            // Arrange
            GenerateAndRegisterStub<IArticleRepository>();
            var sut = new Cart();
            sut.AddItem(1, 1, DateTime.Today, DateTime.Today.AddDays(1));

            // Act
            bool actual = sut.AreItemsAvailable;

            // Assert
            Assert.That(actual, Is.False);
        }
    }
}
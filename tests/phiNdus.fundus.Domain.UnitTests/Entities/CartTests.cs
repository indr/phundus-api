using System;
using NUnit.Framework;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.Domain.Repositories;
using phiNdus.fundus.TestHelpers.TestBases;

namespace phiNdus.fundus.Domain.UnitTests.Entities
{
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
            var actual = sut.AreItemsAvailable;

            // Assert
            Assert.That(actual, Is.False);
        }
    }
}
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.Services;
using phiNdus.fundus.TestHelpers.TestBases;

namespace phiNdus.fundus.Core.Business.IntegrationTests.Services
{
    [TestFixture]
    public class CartServiceTests : ComponentTestBase<OrderService>
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            Sut = new OrderService();
        }

        #endregion

        [Test]
        [Ignore]
        public void GetCart_AfterAddItem_HasItems()
        {
            // Arrange

            // Act
            Sut.AddToCart(new OrderItemDto {Amount = 1, ArticleId = 1});
            Sut.AddToCart(new OrderItemDto {Amount = 2, ArticleId = 2});
            var cart = Sut.GetCart();

            // Assert
            Assert.That(cart.Items, Has.Count.EqualTo(2));
        }
    }
}
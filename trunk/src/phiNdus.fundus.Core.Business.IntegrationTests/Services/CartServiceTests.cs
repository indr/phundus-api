using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.SecuredServices;
using phiNdus.fundus.Core.Business.Services;

namespace phiNdus.fundus.Core.Business.IntegrationTests.Services
{
    [TestFixture]
    public class CartServiceTests : BaseTestFixture<OrderService>
    {
        [SetUp]
        public override void SetUp()
        {
            Sut = new OrderService();
        }

        [Test]
        [Ignore]
        public void GetCart_AfterAddItem_HasItems()
        {
            // Arrange

            // Act
            Sut.AddToCart(new OrderItemDto { Amount = 1, ArticleId = 1 });
            Sut.AddToCart(new OrderItemDto { Amount = 2, ArticleId = 2 });
            var cart = Sut.GetCart();

            // Assert
            Assert.That(cart.Items, Has.Count.EqualTo(2));
        }
    }
}

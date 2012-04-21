using System;
using NUnit.Framework;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Business.Security;
using phiNdus.fundus.Business.Services;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.TestHelpers.Builders;
using phiNdus.fundus.TestHelpers.TestBases;
using Rhino.Commons;

namespace phiNdus.fundus.Business.IntegrationTests.Services
{

    

    [TestFixture]
    public class CartServiceTests : BusinessComponentTestBase<OrderService>
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            using (var uow = UnitOfWork.Start())
            {
                _user = new UserBuilder().Build();
                _article1 = new ArticleBuilder().Build();
                _article2 = new ArticleBuilder().Build();
                uow.TransactionalFlush();
            }

            Sut = new OrderService();
            Sut.SecurityContext = new SecurityContextBuilder().ForUser(_user).Build();
        }

        private User _user;
        private Article _article1;
        private Article _article2;

        #endregion

        [Test]
        public void GetCart_AfterAddItem_HasItems()
        {
            // Arrange


            // Act
            Sut.AddToCart(new OrderItemDto { Amount = 1, ArticleId = _article1.Id, From = DateTime.Today, To = DateTime.Today});
            Sut.AddToCart(new OrderItemDto { Amount = 2, ArticleId = _article2.Id, From = DateTime.Today, To = DateTime.Today });
            var cart = Sut.GetCart();

            // Assert
            Assert.That(cart.Items, Has.Count.EqualTo(2));
        }
    }
}
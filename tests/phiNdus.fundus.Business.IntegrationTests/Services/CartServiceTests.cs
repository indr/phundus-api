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
    public class CartServiceTests : BusinessComponentTestBase<CartService>
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            using (var uow = UnitOfWork.Start())
            {
                _organization = new OrganizationBuilder().Build();
                _user = new UserBuilder().Chief(_organization).Build();
                _article1 = new ArticleBuilder().Organization(_organization).Build();
                _article2 = new ArticleBuilder().Organization(_organization).Build();
                uow.TransactionalFlush();
            }

            Sut = new CartService();
            Sut.SecurityContext = new SecurityContextBuilder().ForUser(_user).Build();
        }

        private User _user;
        private Article _article1;
        private Article _article2;
        private Organization _organization;

        #endregion

        [Test]
        public void GetCart_AfterAddItem_HasItems()
        {
            Assert.Ignore("TODO");

            // Arrange


            // Act
            Sut.AddItem(new CartItemDto { Quantity = 1, ArticleId = _article1.Id, From = DateTime.Today, To = DateTime.Today });
            Sut.AddItem(new CartItemDto { Quantity = 2, ArticleId = _article2.Id, From = DateTime.Today, To = DateTime.Today });
            var cart = Sut.GetCart(null);

            // Assert
            Assert.That(cart.Items, Has.Count.EqualTo(2));
        }
    }
}
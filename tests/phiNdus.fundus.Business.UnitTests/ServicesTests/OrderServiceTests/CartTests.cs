using System;
using NUnit.Framework;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Business.Security;
using phiNdus.fundus.Business.Services;
using phiNdus.fundus.Business.UnitTests.Security;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.Domain.Repositories;
using phiNdus.fundus.TestHelpers;
using phiNdus.fundus.TestHelpers.Builders;
using phiNdus.fundus.TestHelpers.TestBases;
using Rhino.Mocks;

namespace phiNdus.fundus.Business.UnitTests.ServicesTests.OrderServiceTests
{
    [TestFixture]
    public class CartServiceTests : UnitTestBase<CartService>
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            Sut = new CartService();
            Sut.SecurityContext = SecurityContext(1, "key");
        }

        #endregion

        protected SecurityContext SecurityContext(int userId, string key)
        {
            var result = new SecurityContext();
            result.SecuritySession = new SecuritySessionBuilder(new User(userId), key).Build();
            return result;
        }

        [Test]
        public void AddToCart_WithAmountZero_Throws()
        {
            Assert.Ignore("Todo");

            // Act
            var ex = Assert.Throws<ArgumentException>(() => Sut.AddItem(new CartItemDto {Quantity = 0, ArticleId = 1}));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Amount cannot be less or equal 0"));
        }

        [Test]
        public void AddToCart_WithArticleIdZero_Throws()
        {
            Assert.Ignore("Todo");

            // Act
            var ex = Assert.Throws<ArgumentException>(() => Sut.AddItem(new CartItemDto {Quantity = 1, ArticleId = 0}));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("ArticleId cannot be less or equal 0"));
        }

        [Test]
        public void AddToCart_WithNullSubject_Throws()
        {
            Assert.Ignore("Todo");

            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => Sut.AddItem(null));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("orderItemDto"));
        }

        [Test]
        public void AddToCart_AddsNewItem()
        {
            Assert.Ignore("Todo");

            // Arrange
            var order = new Order(1, 2);
            order.Items.Add(new OrderItem());
            GenerateAndRegisterStubUnitOfWork();
            GenerateAndRegisterStub<IOrderRepository>().Expect(x => x.Get(1)).Return(order);
            GenerateAndRegisterStub<IArticleRepository>().Expect(x => x.Get(1)).Return(new Article(1, 2));

            // Act
            Sut.AddItem(new CartItemDto {Quantity = 1, ArticleId = 1});

            // Assert
            Assert.That(order.Items, Has.Count.EqualTo(2));
        }

        [Test]
        public void GetCart_ReturnsCartFromRepository()
        {
            Assert.Ignore("TODO");
            //// Arrange
            //GenerateAndRegisterStubUnitOfWork();
            //GenerateAndRegisterStub<ICartRepository>().Expect(x => x.FindByCustomer(SecurityContext()).Return(new Cart());

            //// Act
            //var cartDto = Sut.GetCart(null);

            //// Assert
            //Assert.That(cartDto, Is.Not.Null);
            //Assert.That(cartDto.Id, Is.EqualTo(1));
            //Assert.That(cartDto.Version, Is.EqualTo(2));
        }

        [Test]
        public void GetCart_WithoutCartInRepository_ReturnsNewCart()
        {
            // Arrange
            GenerateAndRegisterStubUnitOfWork();
            GenerateAndRegisterStub<IOrderRepository>();

            // Act
            var cartDto = Sut.GetCart(null);

            // Assert
            Assert.That(cartDto, Is.Not.Null);
            Assert.That(cartDto.Id, Is.EqualTo(0));
        }
    }
}
using System;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.Security;
using phiNdus.fundus.Core.Business.Services;
using phiNdus.fundus.Core.Business.UnitTests.Security;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using phiNdus.fundus.TestHelpers;
using phiNdus.fundus.TestHelpers.TestBases;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Business.UnitTests.ServicesTests.OrderServiceTests
{
    [TestFixture]
    public class CartTests : UnitTestBase<OrderService>
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            Sut = new OrderService();
            Sut.SecurityContext = SecurityContext(1, "key");
        }

        #endregion

        protected SecurityContext SecurityContext(int userId, string key)
        {
            var result = new SecurityContext();
            result.SecuritySession = SessionHelper.CreateSession(new User(userId), key);
            return result;
        }

        [Test]
        public void AddToCart_WithAmountZero_Throws()
        {
            // Act
            var ex = Assert.Throws<ArgumentException>(() => Sut.AddToCart(new OrderItemDto {Amount = 0, ArticleId = 1}));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Amount cannot be less or equal 0"));
        }

        [Test]
        public void AddToCart_WithArticleIdZero_Throws()
        {
            // Act
            var ex = Assert.Throws<ArgumentException>(() => Sut.AddToCart(new OrderItemDto {Amount = 1, ArticleId = 0}));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("ArticleId cannot be less or equal 0"));
        }

        [Test]
        public void AddToCart_WithNullSubject_Throws()
        {
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => Sut.AddToCart(null));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("orderItemDto"));
        }

        [Test]
        public void AddToCart_AddsNewItem()
        {
            // Arrange
            var order = new Order(1, 2);
            order.Items.Add(new OrderItem());
            GenerateAndRegisterStubUnitOfWork();
            GenerateAndRegisterStub<IOrderRepository>().Expect(x => x.Get(1)).Return(order);
            GenerateAndRegisterStub<IArticleRepository>().Expect(x => x.Get(1)).Return(new Article(1, 2));

            // Act
            Sut.AddToCart(new OrderItemDto {OrderId = 1, Amount = 1, ArticleId = 1});

            // Assert
            Assert.That(order.Items, Has.Count.EqualTo(2));
        }

        [Test]
        public void GetCart_ReturnsCartFromRepository()
        {
            // Arrange
            GenerateAndRegisterStubUnitOfWork();
            GenerateAndRegisterStub<IOrderRepository>().Expect(x => x.FindCart(1)).Return(new Order(1, 2));

            // Act
            var cartDto = Sut.GetCart();

            // Assert
            Assert.That(cartDto, Is.Not.Null);
            Assert.That(cartDto.Id, Is.EqualTo(1));
            Assert.That(cartDto.Version, Is.EqualTo(2));
        }

        [Test]
        public void GetCart_WithoutCartInRepository_ReturnsNewCart()
        {
            // Arrange
            GenerateAndRegisterStubUnitOfWork();
            GenerateAndRegisterStub<IOrderRepository>();

            // Act
            var cartDto = Sut.GetCart();

            // Assert
            Assert.That(cartDto, Is.Not.Null);
            Assert.That(cartDto.Id, Is.EqualTo(0));
        }
    }
}
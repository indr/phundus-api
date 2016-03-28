namespace Phundus.Shop.Model.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using Inventory.Application;
    using Products;

    public class PlaceOrderService
    {
        private readonly IProductsService _productService;
        private readonly IAvailabilityQueryService _availabilityQueryService;

        public PlaceOrderService(IProductsService productService, IAvailabilityQueryService availabilityQueryService)
        {
            _productService = productService;
            _availabilityQueryService = availabilityQueryService;
        }

        public Order PlaceOrder(OrderId orderId, OrderShortId orderShortId, Lessor lessor, Lessee lessee, ICollection<CartItem> cartItems)
        {
            AssertionConcern.AssertArgumentNotEmpty(cartItems, "Cart items must not be empty.");

            ValidateCartItems(lessor.LessorId, lessee.LesseeId, cartItems);
            CheckAvailability(cartItems);

            var orderLines = new OrderLines(cartItems);
            var order = new Order(orderId, orderShortId, lessor, lessee, orderLines);
            order.Place(new Initiator(new UserId(lessee.LesseeId.Id), lessee.EmailAddress, lessee.FullName));
            return order;
        }

        private void CheckAvailability(ICollection<CartItem> cartItems)
        {
            foreach (var grp in cartItems.GroupBy(ks => ks.ArticleId))
            {
                var quantityPeriods = grp.Select(s => new QuantityPeriod(s.Period, s.Quantity, s.CartItemId.Id)).ToList();
                var availabilitiyInfos = _availabilityQueryService.IsAvailable(grp.Key, quantityPeriods);
                if (!availabilitiyInfos.All(p => p.IsAvailable))
                {
                    throw new Exception(String.Format("Product {0} is not available.", grp.Key));
                }
            }
        }

        private void ValidateCartItems(LessorId lessorId, LesseeId lesseeId, IEnumerable<CartItem> cartItems)
        {
            foreach (var each in cartItems)
            {
                var product = _productService.GetById(lessorId, each.ArticleId, lesseeId);

                if (product.Name != each.LineText)
                    throw new StaleDataException("The cart item text is out of date.");
                if (product.Price != each.UnitPrice)
                    throw new StaleDataException("The cart item price is out of date.");
            }
        }
    }
}
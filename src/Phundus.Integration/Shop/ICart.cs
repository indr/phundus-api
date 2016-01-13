namespace Phundus.Integration.Shop
{
    using System;
    using System.Collections.Generic;

    public interface ICart
    {
        int CartId { get; }
        Guid CartGuid { get; }
        int UserId { get; }
        Guid UserGuid { get; }
        IList<ICartItem> Items { get; }
        decimal Total { get; }
    }

    public interface ICartItem
    {
        int CartItemId { get; }
        Guid CartItemGuid { get; }

        int Position { get; }
        int ArticleId { get; }
        string Text { get; }
        DateTime FromUtc { get; }
        DateTime ToUtc { get; }
        int Quantity { get; }
        decimal UnitPricePerWeek { get; }
        decimal ItemTotal { get; }
        int Days { get; }
    }
}
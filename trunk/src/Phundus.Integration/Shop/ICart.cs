namespace Phundus.Integration.Shop
{
    using System;
    using System.Collections.Generic;

    public interface ICart
    {
        Guid CartGuid { get; }
        Guid UserGuid { get; }
        IList<ICartItem> Items { get; }
        decimal Total { get; }
    }

    public interface ICartItem
    {
        Guid CartItemGuid { get; }

        int Position { get; }
        int ArticleId { get; }
        Guid ArticleGuid { get; }
        string Text { get; }
        DateTime FromUtc { get; }
        DateTime ToUtc { get; }
        int Quantity { get; }
        decimal UnitPricePerWeek { get; }
        decimal ItemTotal { get; }
        int Days { get; }

        Guid OwnerGuid { get; }
        string OwnerType { get; }
        string OwnerName { get; }
    }
}
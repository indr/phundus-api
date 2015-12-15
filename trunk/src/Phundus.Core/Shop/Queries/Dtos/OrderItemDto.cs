namespace Phundus.Core.Shop.Queries
{
    using System;

    public partial class OrderItemDto
    {
        partial void OnLoaded()
        {
            _From = DateTime.SpecifyKind(_From, DateTimeKind.Utc);
            _To = DateTime.SpecifyKind(_To, DateTimeKind.Utc);
        }

        public bool IsAvailable { get; set; }
    }
}
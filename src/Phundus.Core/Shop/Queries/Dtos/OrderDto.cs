namespace Phundus.Core.Shop.Queries
{
    using System;
    using System.Linq;

    public partial class OrderDto
    {
        partial void OnLoaded()
        {
            _CreatedOn = DateTime.SpecifyKind(_CreatedOn, DateTimeKind.Utc);
            if (_ModifiedOn.HasValue)
                _ModifiedOn = DateTime.SpecifyKind(_ModifiedOn.Value, DateTimeKind.Utc);
        }

        public decimal TotalPrice
        {
            get { return Items.Sum(p => p.ItemTotal); }
        }
    }
}
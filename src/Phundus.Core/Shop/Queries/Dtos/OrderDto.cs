namespace Phundus.Core.Shop.Queries
{
    using System;

    public partial class OrderDto
    {
        partial void OnLoaded()
        {
            _CreatedOn = DateTime.SpecifyKind(_CreatedOn, DateTimeKind.Utc);
            if (_ModifiedOn.HasValue)
                _ModifiedOn = DateTime.SpecifyKind(_ModifiedOn.Value, DateTimeKind.Utc);
        }
    }
}
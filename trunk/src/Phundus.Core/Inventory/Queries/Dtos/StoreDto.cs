namespace Phundus.Core.Inventory.Queries
{
    using System;

    public class StoreDto
    {
        public Guid StoreId { get; set; }
        public string Address { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string OpeningHours { get; set; }
    }
}
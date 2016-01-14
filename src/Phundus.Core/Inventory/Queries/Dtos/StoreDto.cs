namespace Phundus.Inventory.Queries
{
    using Common.Domain.Model;

    public class StoreDto
    {
        public StoreId StoreId { get; set; }
        public string Address { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string OpeningHours { get; set; }
    }
}
namespace Phundus.Core.Inventory.Queries
{
    using System.Collections.Generic;
    using System.Linq;
    using Services;

    public class AvailabilityReadModel : IAvailabilityQueries
    {
        public IAvailabilityService AvailabilityService { get; set; }

        public IEnumerable<AvailabilityDto> GetAvailability(int id)
        {
            var availabilities = AvailabilityService.GetAvailabilityDetails(id);
            return availabilities.Select(each => new AvailabilityDto {FromUtc = each.FromUtc, Amount = each.Amount});
        }
    }
}
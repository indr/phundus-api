namespace Phundus.Inventory.Queries
{
    using System;
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

        public IEnumerable<AvailabilityDto> GetAvailability(Guid guid)
        {
            var availabilities = AvailabilityService.GetAvailabilityDetails(guid);
            return availabilities.Select(each => new AvailabilityDto { FromUtc = each.FromUtc, Amount = each.Amount });
        }
    }
}
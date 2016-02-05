namespace Phundus.Inventory.Queries
{
    using System;
    using System.Collections.Generic;

    public interface IAvailabilityQueries
    {
        IEnumerable<AvailabilityDto> GetAvailability(int id);
        IEnumerable<AvailabilityDto> GetAvailability(Guid guid);
    }
}
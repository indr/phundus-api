namespace Phundus.Inventory.Queries
{
    using System.Collections.Generic;

    public interface IAvailabilityQueries
    {
        IEnumerable<AvailabilityDto> GetAvailability(int id);
    }
}
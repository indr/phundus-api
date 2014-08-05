namespace Phundus.Core.Inventory.Queries
{
    using System.Collections.Generic;

    public interface IAvailabilityQueries
    {
        IEnumerable<AvailabilityDto> GetAvailability(int id);
    }
}
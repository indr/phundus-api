﻿namespace Phundus.Inventory.Projections
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Cqrs;
    using Services;

    public interface IAvailabilityQueries
    {
        IEnumerable<AvailabilityData> GetAvailability(int id);
        IEnumerable<AvailabilityData> GetAvailability(Guid guid);
    }

    public class AvailabilityProjection : ProjectionBase, IAvailabilityQueries
    {
        public IAvailabilityService AvailabilityService { get; set; }

        public IEnumerable<AvailabilityData> GetAvailability(int id)
        {
            var availabilities = AvailabilityService.GetAvailabilityDetails(id);
            return availabilities.Select(each => new AvailabilityData {FromUtc = each.FromUtc, Amount = each.Amount});
        }

        public IEnumerable<AvailabilityData> GetAvailability(Guid guid)
        {
            var availabilities = AvailabilityService.GetAvailabilityDetails(guid);
            return availabilities.Select(each => new AvailabilityData { FromUtc = each.FromUtc, Amount = each.Amount });
        }
    }

    public class AvailabilityData
    {
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
        public DateTime FromUtc { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime FromLocal { get { return FromUtc.ToLocalTime(); } }

        public int Amount { get; set; }
    }
}
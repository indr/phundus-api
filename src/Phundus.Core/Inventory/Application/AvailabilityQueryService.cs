﻿namespace Phundus.Inventory.Application
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Common.Domain.Model;
    using Common.Querying;

    public interface IAvailabilityQueryService
    {
        IEnumerable<AvailabilityData> GetAvailability(ArticleId guid);

        bool IsArticleAvailable(ArticleId articleId, DateTime fromUtc, DateTime toUtc, int quantity,
            OrderLineId orderItemToExclude = null);
    }

    public class AvailabilityQueryService : QueryServiceBase, IAvailabilityQueryService
    {
        public IAvailabilityService AvailabilityService { get; set; }

        public IEnumerable<AvailabilityData> GetAvailability(ArticleId guid)
        {
            var availabilities = AvailabilityService.GetAvailabilityDetails(guid);
            return availabilities.Select(each => new AvailabilityData {FromUtc = each.FromUtc, Quantity = each.Quantity});
        }

        public bool IsArticleAvailable(ArticleId articleId, DateTime fromUtc, DateTime toUtc, int quantity,
            OrderLineId orderItemToExclude = null)
        {
            return AvailabilityService.IsArticleAvailable(articleId, fromUtc, toUtc, quantity, orderItemToExclude);
        }
    }

    public class AvailabilityData
    {
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
        public DateTime FromUtc { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime FromLocal
        {
            get { return FromUtc.ToLocalTime(); }
        }

        public int Quantity { get; set; }
    }
}
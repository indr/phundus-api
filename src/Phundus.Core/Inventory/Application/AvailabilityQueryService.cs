namespace Phundus.Inventory.Application
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using Common.Querying;
    using Itenso.TimePeriod;
    using Model.Reservations;

    public interface IAvailabilityQueryService
    {
        IEnumerable<AvailabilityData> GetAvailability(ArticleId guid);

        bool IsArticleAvailable(ArticleId articleId, DateTime fromUtc, DateTime toUtc, int quantity,
            OrderLineId orderItemToExclude = null);

        bool IsAvailable(ArticleId productId, ICollection<QuantityPeriod> quantityPeriods);
    }

    public class AvailabilityQueryService : QueryServiceBase, IAvailabilityQueryService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IAvailabilityService _availabilityService;

        public AvailabilityQueryService(IReservationRepository reservationRepository, IAvailabilityService availabilityService)
        {
            _reservationRepository = reservationRepository;
            _availabilityService = availabilityService;
        }

        public IEnumerable<AvailabilityData> GetAvailability(ArticleId guid)
        {
            var quantityPeriods = GetAvailabilities(guid);
            return quantityPeriods.Intervals.Select(s => new AvailabilityData {FromUtc = s.Start, Quantity = s.Quantity});
        }

        public bool IsArticleAvailable(ArticleId articleId, DateTime fromUtc, DateTime toUtc, int quantity,
            OrderLineId orderItemToExclude = null)
        {
            return _availabilityService.IsArticleAvailable(articleId, fromUtc, toUtc, quantity, orderItemToExclude);
        }

        public bool IsAvailable(ArticleId productId, ICollection<QuantityPeriod> quantityPeriods)
        {
            var stockAvailable = GetAvailabilities(productId);

            foreach (var each in quantityPeriods)
            {
                stockAvailable.Add(each.Start, each.End, each.Quantity * -1);
            }

            return stockAvailable.Intervals.All(p => p.Quantity >= 0);
        }

        private QuantityPeriods GetAvailabilities(ArticleId productId)
        {
            var result = new QuantityPeriods();
            var availabilities = _availabilityService.GetAvailabilityDetails(productId).ToList();
            for (var i = 0; i < availabilities.Count; i++)
            {
                var each = availabilities[i];
                ITimePeriod interval;
                if (i == availabilities.Count - 1)
                {
                    interval = new TimeInterval(each.FromUtc, DateTime.MaxValue, IntervalEdge.Closed, IntervalEdge.Open, true, true);
                }
                else
                {
                    var end = availabilities[i + 1].FromUtc;
                    interval = new TimeInterval(each.FromUtc, end, IntervalEdge.Closed, IntervalEdge.Open, true, true);
                    
                }
                result.Add(interval.Start, interval.End, each.Quantity);                
            }

            return result;
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
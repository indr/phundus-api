namespace Phundus.Inventory.Application
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Articles.Model;
    using Articles.Repositories;
    using AvailabilityAndReservation.Model;
    using Common.Domain.Model;
    using Infrastructure;
    using Model.Reservations;

    public interface IAvailabilityService
    {
        bool IsArticleAvailable(ArticleId articleId, DateTime fromUtc, DateTime toUtc, int quantity, OrderLineId orderItemToExclude = null);
        
        IEnumerable<Availability> GetAvailabilityDetails(ArticleId articleId);
    }

    public class AvailabilityService : IAvailabilityService
    {
        public IArticleRepository ArticleRepository { get; set; }

        public IReservationRepository ReservationRepository { get; set; }

        public bool IsArticleAvailable(ArticleId articleId, DateTime fromUtc, DateTime toUtc, int quantity, OrderLineId orderLineIdToExclude)
        {
            return IsArticleAvailable(articleId, new Period(fromUtc, toUtc), quantity, orderLineIdToExclude);
        }

        private bool IsArticleAvailable(ArticleId article, Period period, int quantity, OrderLineId orderLineIdToExclude)
        {
            if (article == null) throw new ArgumentNullException("article");
            if (period == null) throw new ArgumentNullException("period");


            var availabilities = GetAvailabilityDetails(article, orderLineIdToExclude);

            var inRange = availabilities.Where(p => p.FromUtc <= period.ToUtc).OrderByDescending(x => x.FromUtc);

            foreach (var each in inRange)
            {
                if ((each.FromUtc >= period.FromUtc) && (each.Quantity < quantity))
                    return false;

                if ((each.FromUtc <= period.FromUtc))
                {
                    return each.Quantity >= quantity;
                }
            }
            return false;
        }

        public IEnumerable<Availability> GetAvailabilityDetails(ArticleId articleId)
        {
            return GetAvailabilityDetails(articleId, null);
        }

        private IEnumerable<Availability> GetAvailabilityDetails(ArticleId articleId, OrderLineId orderLineIdToExclude)
        {
            var localTodayUtc = DateTimeProvider.Today.ToUniversalTime();
            var utcNow = DateTimeProvider.UtcNow;

            var result = new List<Availability>();
            var article = ArticleRepository.FindById(articleId);
            if (article == null)
                return result;

            var reservations = ReservationRepository.Find(articleId, orderLineIdToExclude).OrderBy(x => x.FromUtc);

            var diffsAt = new Dictionary<DateTime, int>();
            diffsAt[DateTime.MinValue] = article.GrossStock;
            foreach (var each in reservations)
            {
                int diffAt;
                diffsAt.TryGetValue(each.FromUtc, out diffAt);
                diffsAt[each.FromUtc] = diffAt - each.Quantity;

                var toUtc = each.ToUtc.AddSeconds(1);
                diffsAt.TryGetValue(toUtc, out diffAt);
                diffsAt[toUtc] = diffAt + each.Quantity;
            }

            var diffsAtSorted = diffsAt.OrderBy(x => x.Key);


            var currentQuantity = 0;

            foreach (var each in diffsAtSorted)
            {
                if (each.Value == 0)
                    continue;


                currentQuantity = currentQuantity + each.Value;
                if (each.Key < localTodayUtc)
                    continue;

                if ((result.Count == 0) && (each.Key > localTodayUtc))
                    result.Add(new Availability {FromUtc = localTodayUtc, Quantity = currentQuantity - each.Value});


                result.Add(new Availability {FromUtc = each.Key, Quantity = currentQuantity});
            }

            if (result.Count == 0)
                result.Insert(0, new Availability {FromUtc = localTodayUtc, Quantity = article.GrossStock});

            return result;
        }
    }
}
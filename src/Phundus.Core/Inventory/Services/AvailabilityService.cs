namespace Phundus.Inventory.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Articles.Model;
    using Articles.Repositories;
    using AvailabilityAndReservation.Model;
    using AvailabilityAndReservation.Repositories;
    using Common.Domain.Model;
    using Infrastructure;

    public interface IAvailabilityService
    {
        bool IsArticleAvailable(int articleId, DateTime fromUtc, DateTime toUtc, int quantity);
        bool IsArticleAvailable(Guid articleGuid, DateTime fromUtc, DateTime toUtc, int quantity);
        bool IsArticleAvailable(int articleId, DateTime fromUtc, DateTime toUtc, int amount, Guid orderItemToExclude);
        bool IsArticleAvailable(Guid articleGuid, DateTime fromUtc, DateTime toUtc, int amount, Guid orderItemToExclude);
        
        IEnumerable<Availability> GetAvailabilityDetails(int articleId);
        IEnumerable<Availability> GetAvailabilityDetails(Guid articleGuid);
    }

    public class AvailabilityService : IAvailabilityService
    {
        public IArticleRepository ArticleRepository { get; set; }

        public IReservationRepository ReservationRepository { get; set; }

        public bool IsArticleAvailable(int articleId, DateTime fromUtc, DateTime toUtc, int quantity)
        {
            return IsArticleAvailable(articleId, fromUtc, toUtc, quantity, new Guid());
        }

        public bool IsArticleAvailable(Guid articleGuid, DateTime fromUtc, DateTime toUtc, int quantity)
        {
            return IsArticleAvailable(articleGuid, fromUtc, toUtc, quantity, new Guid());
        }

        public bool IsArticleAvailable(int articleId, DateTime fromUtc, DateTime toUtc, int amount,
            Guid orderItemToExclude)
        {
            var article = ArticleRepository.FindById(new ArticleShortId(articleId));
            return IsArticleAvailable(article, new Period(fromUtc, toUtc), amount, orderItemToExclude);
        }

        public bool IsArticleAvailable(Guid articleGuid, DateTime fromUtc, DateTime toUtc, int amount, Guid orderItemToExclude)
        {
            var article = ArticleRepository.FindById(new ArticleId(articleGuid));
            return IsArticleAvailable(article, new Period(fromUtc, toUtc), amount, orderItemToExclude);
        }

        private bool IsArticleAvailable(Article article, Period period, int amount, Guid orderItemToExclude)
        {
            if (article == null) throw new ArgumentNullException("article");
            if (period == null) throw new ArgumentNullException("period");


            var availabilities = GetAvailabilityDetails(article.Id, orderItemToExclude);

            var inRange = availabilities.Where(p => p.FromUtc <= period.ToUtc).OrderByDescending(x => x.FromUtc);

            foreach (var each in inRange)
            {
                if ((each.FromUtc >= period.FromUtc) && (each.Amount < amount))
                    return false;

                if ((each.FromUtc <= period.FromUtc))
                {
                    return each.Amount >= amount;
                }
            }
            return false;
        }

        public IEnumerable<Availability> GetAvailabilityDetails(int articleId)
        {
            return GetAvailabilityDetails(articleId, Guid.Empty);
        }

        public IEnumerable<Availability> GetAvailabilityDetails(Guid articleGuid)
        {
            var article = ArticleRepository.GetById(new ArticleId(articleGuid));
            return GetAvailabilityDetails(article.Id);
        }

        private IEnumerable<Availability> GetAvailabilityDetails(int articleId, Guid orderItemToExclude)
        {
            var localTodayUtc = DateTimeProvider.Today.ToUniversalTime();
            var utcNow = DateTimeProvider.UtcNow;

            var result = new List<Availability>();
            var article = ArticleRepository.FindById(new ArticleShortId(articleId));
            if (article == null)
                return result;

            var reservations = ReservationRepository.Find(articleId, orderItemToExclude).OrderBy(x => x.FromUtc);

            var diffsAt = new Dictionary<DateTime, int>();
            diffsAt[DateTime.MinValue] = article.GrossStock;
            foreach (var each in reservations)
            {
                int diffAt;
                diffsAt.TryGetValue(each.FromUtc, out diffAt);
                diffsAt[each.FromUtc] = diffAt - each.Amount;

                var toUtc = each.ToUtc.AddSeconds(1);
                diffsAt.TryGetValue(toUtc, out diffAt);
                diffsAt[toUtc] = diffAt + each.Amount;
            }

            var diffsAtSorted = diffsAt.OrderBy(x => x.Key);


            var currentAmount = 0;

            foreach (var each in diffsAtSorted)
            {
                if (each.Value == 0)
                    continue;


                currentAmount = currentAmount + each.Value;
                if (each.Key < localTodayUtc)
                    continue;

                if ((result.Count == 0) && (each.Key > localTodayUtc))
                    result.Add(new Availability {FromUtc = localTodayUtc, Amount = currentAmount - each.Value});


                result.Add(new Availability {FromUtc = each.Key, Amount = currentAmount});
            }

            if (result.Count == 0)
                result.Insert(0, new Availability {FromUtc = localTodayUtc, Amount = article.GrossStock});

            return result;
        }
    }
}
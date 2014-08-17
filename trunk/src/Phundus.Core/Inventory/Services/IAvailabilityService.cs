namespace Phundus.Core.Inventory.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Articles.Repositories;
    using AvailabilityAndReservation._Legacy;
    using Infrastructure;
    using NHibernate;
    using ReservationCtx.Repositories;

    public interface IAvailabilityService
    {
        bool IsArticleAvailable(int articleId, DateTime fromUtc, DateTime toUtc, int amount);
        IEnumerable<Availability> GetAvailability(int articleId);
    }

    public class AvailabilityService : IAvailabilityService
    {
        public IArticleRepository ArticleRepository { get; set; }
        
        public IReservationRepository ReservationRepository { get; set; }
        
        public Func<ISession> SessionFactory { get; set; }

        public bool IsArticleAvailable(int articleId, DateTime fromUtc, DateTime toUtc, int amount)
        {
            var article = ArticleRepository.GetById(articleId);

            return new AvailabilityChecker(article, SessionFactory()).Check(fromUtc, toUtc, amount);
        }

        public IEnumerable<Availability> GetAvailability(int articleId)
        {
            var utcToday = DateTimeProvider.UtcToday;
            var utcNow = DateTimeProvider.UtcNow;
            var article = ArticleRepository.GetById(articleId);
            var reservations = ReservationRepository.Find(articleId).OrderBy(x => x.FromUtc);
            var result = new List<Availability>();


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

                if (each.Key < utcToday)
                    continue;

                if ((result.Count == 0) && (each.Key > utcNow))
                    result.Add(new Availability {FromUtc = utcToday, Amount = article.GrossStock});
                result.Add(new Availability { FromUtc = each.Key, Amount = currentAmount });
            }

            if (result.Count == 0)
                result.Insert(0, new Availability { FromUtc = utcToday, Amount = article.GrossStock });

            return result;
        }
    }
}
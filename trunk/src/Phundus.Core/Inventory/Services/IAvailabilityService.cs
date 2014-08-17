namespace Phundus.Core.Inventory.Services
{
    using System;
    using System.Collections.Generic;
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
            var article = ArticleRepository.GetById(articleId);
            var reservations = ReservationRepository.Find(articleId);
            return new List<Availability> {new Availability {FromUtc = DateTimeProvider.UtcToday, Amount = article.GrossStock}};
        }
    }
}
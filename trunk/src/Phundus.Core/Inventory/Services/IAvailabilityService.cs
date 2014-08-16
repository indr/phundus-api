namespace Phundus.Core.Inventory.Services
{
    using System;
    using Articles.Repositories;
    using AvailabilityAndReservation._Legacy;
    using NHibernate;

    public interface IAvailabilityService
    {
        bool IsArticleAvailable(int articleId, DateTime fromUtc, DateTime toUtc, int amount);
    }

    public class AvailabilityService : IAvailabilityService
    {
        public IArticleRepository ArticleRepository { get; set; }

        public Func<ISession> SessionFactory { get; set; } 

        public bool IsArticleAvailable(int articleId, DateTime fromUtc, DateTime toUtc, int amount)
        {
            var article = ArticleRepository.GetById(articleId);

            return new AvailabilityChecker(article, SessionFactory()).Check(fromUtc, toUtc, amount);
        }
    }
}
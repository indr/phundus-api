namespace Phundus.Core.Inventory.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Articles.Repositories;
    using AvailabilityAndReservation._Legacy;
    using NHibernate;

    public class AvailabilityReadModel : IAvailabilityQueries
    {
        public Func<ISession> Session { get; set; }

        public IArticleRepository ArticleRepository { get; set; }

        public IEnumerable<AvailabilityDto> GetAvailability(int id)
        {
            var article = ArticleRepository.ById(id);
            var availabilities = new NetStockCalculator(article, Session())
                .From(DateTime.Today).To(DateTime.Today.AddYears(1));
            return
                availabilities.Select(each => new AvailabilityDto {FromUtc = each.FromUtc, Amount = each.Amount});
        }
    }
}
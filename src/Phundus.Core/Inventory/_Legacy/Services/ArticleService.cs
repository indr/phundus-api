namespace Phundus.Core.Inventory._Legacy.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NHibernate;
    using Queries;
    using Repositories;

    public class ArticleService : AppServiceBase, IArticleService
    {
        public Func<ISession> Session { get; set; }

        public IArticleRepository ArticleRepository { get; set; }

        public IList<ImageDto> GetImages(int articleId)
        {
            var article = ArticleRepository.ById(articleId);
            var assembler = new ImageAssembler();
            return assembler.CreateDtos(article.Images);
        }

        public IList<AvailabilityDto> GetAvailability(int id)
        {
            var article = ArticleRepository.ById(id);
            var availabilities = new NetStockCalculator(article, Session())
                .From(DateTime.Today).To(DateTime.Today.AddYears(1));
            return
                availabilities.Select(each => new AvailabilityDto {Date = each.Date, Amount = each.Amount}).ToList();
        }
    }
}
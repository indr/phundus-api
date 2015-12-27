namespace Phundus.Core.Inventory.Queries
{
    using System;
    using System.Collections.Generic;
    using Articles.Repositories;
    using Cqrs;

    public class ArticleReadModel : ReadModelBase, IArticleQueries
    {
        public IArticleRepository ArticleRepository { get; set; }

        public ArticleDto GetArticle(int id)
        {
            var article = ArticleRepository.FindById(id);
            if (article == null)
                return null;
            return new ArticleDtoAssembler().CreateDto(article);
        }

        public IEnumerable<ArticleDto> GetArticles(Guid organizationId)
        {
            var articles = ArticleRepository.ByOrganization(organizationId);
            return new ArticleDtoAssembler().CreateDtos(articles);
        }
    }
}
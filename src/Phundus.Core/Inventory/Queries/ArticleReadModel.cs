namespace Phundus.Core.Inventory.Queries
{
    using System;
    using System.Collections.Generic;
    using Articles.Repositories;
    using Cqrs;
    using IdentityAndAccess.Organizations.Repositories;

    public class ArticleReadModel : ReadModelBase, IArticleQueries
    {
        public IArticleRepository ArticleRepository { get; set; }

        public IOrganizationRepository OrganizationRepository { get; set; }

        public ArticleDto GetArticle(int id)
        {
            var article = ArticleRepository.FindById(id);
            if (article == null)
                return null;
            return new ArticleDtoAssembler(OrganizationRepository).CreateDto(article);
        }

        public IEnumerable<ArticleDto> GetArticles(Guid organizationId)
        {
            var articles = ArticleRepository.ByOrganization(organizationId);
            return new ArticleDtoAssembler(OrganizationRepository).CreateDtos(articles);
        }
    }
}
namespace Phundus.Core.Inventory.Queries
{
    using System.Collections.Generic;
    using Cqrs;
    using IdentityAndAccess.Organizations.Repositories;
    using Repositories;

    public class ArticleReadModel : ReadModelBase, IArticleQueries
    {
        public IArticleRepository ArticleRepository { get; set; }

        public IOrganizationRepository OrganizationRepository { get; set; }

        public ArticleDto GetArticle(int id)
        {
            var article = ArticleRepository.ById(id);
            if (article == null)
                return null;
            return new ArticleDtoAssembler(OrganizationRepository).CreateDto(article);
        }

        public IEnumerable<ArticleDto> GetArticles(int organizationId)
        {
            var articles = ArticleRepository.ByOrganization(organizationId);
            return new ArticleDtoAssembler(OrganizationRepository).CreateDtos(articles);
        }
    }
}
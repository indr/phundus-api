namespace Phundus.Core.Inventory.Queries
{
    using System.Collections.Generic;
    using Cqrs;
    using Repositories;
    using _Legacy.Assemblers;
    using _Legacy.Dtos;

    public class ArticleReadModel : ReadModelBase, IArticleQueries
    {
        public IArticleRepository ArticleRepository { get; set; }

        public ArticleDto GetArticle(int id)
        {
            var article = ArticleRepository.ById(id);
            if (article == null)
                return null;
            return new ArticleDtoAssembler().CreateDto(article);
        }

        public IEnumerable<ArticleDto> GetArticles(int organizationId)
        {
            var articles = ArticleRepository.ByOrganization(organizationId);
            return new ArticleDtoAssembler().CreateDtos(articles);
        }

        
    }
}
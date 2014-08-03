namespace Phundus.Core.Inventory.Queries
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Cqrs;
    using Cqrs.Paging;
    using InventoryCtx.Repositories;
    using _Legacy.Assemblers;
    using _Legacy.Dtos;

    public interface IArticleQueries
    {
        ArticleDto GetArticle(int id);
        IEnumerable<ArticleDto> GetArticles(int organizationId);
    }

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
            var articles = ArticleRepository.FindAll(organizationId);
            return new ArticleDtoAssembler().CreateDtos(articles);
        }

        
    }
}
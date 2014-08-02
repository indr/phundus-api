namespace Phundus.Core.Inventory.Queries
{
    using System.Linq;
    using Cqrs;
    using Cqrs.Paging;
    using InventoryCtx.Repositories;
    using _Legacy.Assemblers;
    using _Legacy.Dtos;
    using _Legacy.Services;

    public interface IArticleQueries
    {
        ArticleDto GetArticle(int id);
        ArticleDto[] GetArticles(int organizationId);

        PagedResult<ArticleDto> FindArticles(PageRequest pageRequest, string query, int? organization);
    }

    public class ArticleReadModel : ReadModelBase, IArticleQueries
    {
        public IArticleService ArticleService { get; set; }

        public IArticleRepository ArticleRepository { get; set; }

        public ArticleDto GetArticle(int id)
        {
            var article = ArticleRepository.ById(id);
            if (article == null)
                return null;
            return new ArticleDtoAssembler().CreateDto(article);
        }

        public ArticleDto[] GetArticles(int organizationId)
        {
            var articles = ArticleRepository.FindAll(organizationId);
            return new ArticleDtoAssembler().CreateDtos(articles);
        }

        public PagedResult<ArticleDto> FindArticles(PageRequest pageRequest, string query, int? organization)
        {
            int total;
            var result = ArticleRepository.FindMany(query, organization, pageRequest.Index*pageRequest.Size,
                pageRequest.Size,
                out total);
            var dtos = new ArticleDtoAssembler().CreateDtos(result).ToList();
            return new PagedResult<ArticleDto>(PageResponse.From(pageRequest, total), dtos);
        }
    }
}
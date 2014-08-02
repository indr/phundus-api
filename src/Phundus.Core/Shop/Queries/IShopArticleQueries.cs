namespace Phundus.Core.Shop.Queries
{
    using Cqrs;
    using Cqrs.Paging;
    using Inventory.Queries;
    using Inventory._Legacy.Dtos;

    public interface IShopArticleQueries
    {
        ArticleDto GetArticle(int id);
        PagedResult<ArticleDto> FindArticles(PageRequest pageRequest, string query, int? organization);
    }

    public class ShopArticleReadModel : ReadModelBase, IShopArticleQueries
    {
        public IArticleQueries ArticleQueries { get; set; }

        public ArticleDto GetArticle(int id)
        {
            return ArticleQueries.GetArticle(id);
        }

        public PagedResult<ArticleDto> FindArticles(PageRequest pageRequest, string query, int? organization)
        {
            return ArticleQueries.FindArticles(pageRequest, query, organization);
        }
    }
}
namespace Phundus.Core.Inventory.Application.Commands
{
    using Domain.Model.Catalog;
    using Domain.Model.Management;
    using IdentityAndAccess.Domain.Model.Organizations;

    public class AllocationHandlerBase
    {
        public AllocationHandlerBase(IStockRepository stockRepository, IArticleRepository articleRepository)
        {
            StockRepository = stockRepository;
            ArticleRepository = articleRepository;
        }

        protected IStockRepository StockRepository { get; private set; }

        protected IArticleRepository ArticleRepository { get; private set; }

        protected Stock GetStock(OrganizationId organizationId, ArticleId articleId, StockId stockId)
        {
            if (Equals(stockId, StockId.Default))
                return GetDefaultStock(organizationId, articleId);
            return StockRepository.Get(organizationId, stockId);
        }

        private Stock GetDefaultStock(OrganizationId organizationId, ArticleId articleId)
        {
            var article = ArticleRepository.GetById(organizationId.Id, articleId.Id);

            return StockRepository.Get(organizationId, new StockId(article.StockId));
        }
    }
}
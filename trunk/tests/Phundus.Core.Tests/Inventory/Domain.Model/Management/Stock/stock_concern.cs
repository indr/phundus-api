namespace Phundus.Core.Tests.Inventory.Domain.Model.Management.Stock
{
    using Core.IdentityAndAccess.Domain.Model.Organizations;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Inventory.Domain.Model.Management;
    using Machine.Specifications;

    public class stock_concern : aggregate_root_concern<Stock>
    {
        protected static OrganizationId _organizationId = new OrganizationId(1001);
        protected static ArticleId _articleId = new ArticleId(10001);
        protected static StockId _stockId = new StockId("Stock-1");

        private Establish ctx = () =>
        {
            _sut = new Stock(_organizationId, _articleId, _stockId);
            _sut.MutatingEvents.Clear();
        };
    }
}
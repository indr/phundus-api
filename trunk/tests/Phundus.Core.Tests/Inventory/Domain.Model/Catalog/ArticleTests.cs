namespace Phundus.Core.Tests.Inventory.Domain.Model.Catalog
{
    using System;
    using Core.IdentityAndAccess.Domain.Model.Organizations;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Inventory.Domain.Model.Management;
    using Machine.Specifications;

    public class article_create_stock_concern
    {
        protected static Article _sut;
        protected static StockId _stockId = new StockId("Stock-1234");
        protected static Stock _stock;
        protected static ArticleId _articleId;
        protected static OrganizationId _organizationId = new OrganizationId(1001);

        private Establish ctx = () =>
        {
            _articleId = new ArticleId(10001);
            _sut = new Article(_articleId, _organizationId.Id, "Name");
        };
    }

    [Subject(typeof (Article))]
    public class when_create_stock_is_called : article_create_stock_concern
    {
        private Because of = () => { _stock = _sut.CreateStock(_stockId); };

        public It should_create_stock_with_article_id = () => _stock.ArticleId.ShouldEqual(_articleId);

        public It should_create_stock_with_organization_id =
            () => _stock.OrganizationId.ShouldEqual(new OrganizationId(1001));

        public It should_create_stock_with_stock_id = () => _stock.StockId.ShouldEqual(_stockId);

        public It should_have_stock = () => _sut.StockId.ShouldEqual(_stockId.Id);
    }

    [Subject(typeof (Article))]
    public class when_create_stock_with_default_stock_id_is_called : article_create_stock_concern
    {
        private static Exception _exception;
        private Because of = () => _exception = Catch.Exception(() => _sut.CreateStock(StockId.Default));

        public It should_have_message =
            () => _exception.Message.ShouldEqual("Stock id must not be the default stock id.");

        public It should_throw_invalid_operation_exception = () => _exception.ShouldNotBeNull();
    }

    [Subject(typeof (Article))]
    public class when_create_stock_is_called_and_there_is_already_a_stock : article_create_stock_concern
    {
        private static Exception _exception;
        private Establish ctx = () => _sut.CreateStock(_stockId);

        private Because of = () => _exception = Catch.Exception(() => _sut.CreateStock(new StockId("Stock-9999")));

        public It should_have_message =
            () => _exception.Message.ShouldEqual("Article has already a stock. Only one stock is currently supported.");

        public It should_throw_invalid_operation_exception = () => _exception.ShouldNotBeNull();
    }
}
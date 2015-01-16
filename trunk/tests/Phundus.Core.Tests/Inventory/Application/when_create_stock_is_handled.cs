namespace Phundus.Core.Tests.Inventory.Application
{
    using Core.IdentityAndAccess.Domain.Model.Organizations;
    using Core.IdentityAndAccess.Domain.Model.Users;
    using Core.IdentityAndAccess.Queries;
    using Core.Inventory.Application.Commands;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Inventory.Domain.Model.Management;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;

    [Subject(typeof (CreateStockHandler))]
    public class when_create_stock_is_handled : handler_concern<CreateStock, CreateStockHandler>
    {
        private static IStockRepository _stockRepository;
        private static IArticleRepository _articleRepository;
        private static IMemberInRole _memberInRole;
        
        private static OrganizationId _organizationId;
        private static UserId _initiatorId;
        private static StockId _stockId;
        private static ArticleId _articleId;
        private static Article _article;

        public Establish ctx = () =>
        {
            _memberInRole = depends.on<IMemberInRole>();
            _stockRepository = depends.on<IStockRepository>();
            _articleRepository = depends.on<IArticleRepository>();

            _initiatorId = new UserId(11);
            _organizationId = new OrganizationId(1);
            _articleId = new ArticleId(101);
            _stockId = new StockId("Stock-1234");
            _stockRepository.WhenToldTo(x => x.GetNextIdentity()).Return(_stockId);

            _article = mock.partial<Article>(new object[] { _articleId, _organizationId.Id, "Article name" });
            _articleRepository.WhenToldTo(x => x.GetById(_organizationId.Id, _articleId.Id)).Return(_article);

            command = new CreateStock(_initiatorId, _organizationId, _articleId);
        };

        public It should_ask_for_chief_privileges =
            () => _memberInRole.WasToldTo(x => x.ActiveChief(_organizationId, _initiatorId));

        public It should_have_resulting_stock_id = () => command.ResultingStockId.ShouldEqual(_stockId);

        public It should_save_stock = () => _stockRepository.WasToldTo(x => x.Save(Arg<Stock>.Is.NotNull));

        public It should_create_stock_on_article = () => _article.WasToldTo(x => x.CreateStock(_stockId));
        
        public It should_save_article = () => _articleRepository.WasToldTo(x => x.Save(_article));
    }
}
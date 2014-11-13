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
        private static IStockRepository repository;
        private static IMemberInRole memberInRole;

        private static OrganizationId organizationId;
        private static UserId initiatorId;
        private static StockId stockId;
        private static ArticleId articleId;

        public Establish ctx = () =>
        {
            memberInRole = depends.on<IMemberInRole>();
            repository = depends.on<IStockRepository>();

            initiatorId = new UserId(11);
            organizationId = new OrganizationId(1);
            articleId = new ArticleId(101);
            stockId = new StockId("Stock-1234");
            repository.WhenToldTo(x => x.GetNextIdentity()).Return(stockId);

            command = new CreateStock(initiatorId, organizationId, articleId);
        };

        public It should_ask_for_chief_privileges =
            () => memberInRole.WasToldTo(x => x.ActiveChief(organizationId, initiatorId));

        public It should_have_resulting_stock_id = () => command.ResultingStockId.ShouldEqual(stockId);

        public It should_save_to_repository = () => repository.WasToldTo(x => x.Save(Arg<Stock>.Is.NotNull));
    }
}
namespace Phundus.Core.Tests.Inventory.Application
{
    using System;
    using Core.IdentityAndAccess.Domain.Model.Organizations;
    using Core.IdentityAndAccess.Domain.Model.Users;
    using Core.IdentityAndAccess.Queries;
    using Core.Inventory.Application.Commands;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Inventory.Domain.Model.Management;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;

    [Subject(typeof (ChangeQuantityInInventoryHandler))]
    public class when_increase_quantity_in_inventory_is_handled :
        handler_concern<ChangeQuantityInInventory, ChangeQuantityInInventoryHandler>
    {
        private static IMemberInRole memberInRole;
        private static IStockRepository repository;
        private static UserId initiatorId = new UserId(1);
        private static OrganizationId organizationId = new OrganizationId(2);
        private static ArticleId articleId = new ArticleId(3);
        private static StockId stockId = new StockId("Stock-1");
        private static int amount = 10;
        private static Stock stock;

        public Establish ctx = () =>
        {
            memberInRole = depends.on<IMemberInRole>();
            stock = new Stock(stockId, articleId);
            repository = depends.on<IStockRepository>();
            repository.Expect(x => x.Get(organizationId, articleId, stockId)).Return(stock);

            command = new ChangeQuantityInInventory(initiatorId.Id, organizationId.Id, articleId.Id, stockId.Id,
                amount, DateTime.UtcNow, null);
        };

        public It should_ask_for_chief_privileges =
            () =>
                memberInRole.WasToldTo(
                    x => x.ActiveChief(Arg<OrganizationId>.Is.Equal(organizationId), Arg<UserId>.Is.Equal(initiatorId)));

        public It should_save_to_repository = () => repository.WasToldTo(x => x.Save(stock));
    }
}
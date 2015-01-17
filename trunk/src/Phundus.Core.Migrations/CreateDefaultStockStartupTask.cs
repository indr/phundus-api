namespace Phundus.Core.Migrations
{
    using Castle.Transactions;
    using Cqrs;
    using IdentityAndAccess.Domain.Model.Organizations;
    using IdentityAndAccess.Domain.Model.Users;
    using Inventory.Application.Commands;
    using Inventory.Domain.Model.Catalog;

    public class CreateDefaultStockStartupTask : DomainModelMigrationStartupTaskBase
    {
        private readonly IArticleRepository _articleRepository;

        public CreateDefaultStockStartupTask(ICommandDispatcher commandDispatcher, IArticleRepository articleRepository)
            : base(commandDispatcher)
        {
            _articleRepository = articleRepository;
        }

        [Transaction]
        public override void Run()
        {
            var articles = _articleRepository.GetAll();

            foreach (var each in articles)
            {
                if (each.StockId != null)
                    return;

                var organizationId = new OrganizationId(each.OrganizationId);
                var articleId = new ArticleId(each.Id);

                var createStock = new CreateStock(UserId.Root, organizationId, articleId);
                Dispatch(createStock);

                var changeQuantityInInventory = new ChangeQuantityInInventory(UserId.Root, organizationId, articleId,
                    createStock.ResultingStockId, each.GrossStock, each.CreateDate,
                    "Vom System migrierter Anfangsbestand.");
                Dispatch(changeQuantityInInventory);
            }
        }
    }
}
namespace Phundus.Core.Inventory.Application.Commands
{
    using Cqrs;
    using Domain.Model.Catalog;
    using Domain.Model.Management;
    using IdentityAndAccess.Domain.Model.Organizations;
    using IdentityAndAccess.Domain.Model.Users;
    using IdentityAndAccess.Queries;

    public class IncreaseQuantityInInventory
    {
        public IncreaseQuantityInInventory(int initiatorId, int organizationId, int articleId, string stockId, int amount)
        {
            InitiatorId = initiatorId;
            OrganizationId = organizationId;
            ArticleId = articleId;
            StockId = stockId;
            Amount = amount;
        }

        public int InitiatorId { get; private set; }
        public int OrganizationId { get; private set; }
        public int ArticleId { get; private set; }
        public string StockId { get; private set; }
        public int Amount { get; private set; }
    }

    public class IncreaseQuantityInInventoryHandler : IHandleCommand<IncreaseQuantityInInventory>
    {
        public IMemberInRole MemberInRole { get; set; }

        public IStockRepository Repository { get; set; }

        public void Handle(IncreaseQuantityInInventory command)
        {
            var organizationId = new OrganizationId(command.OrganizationId);
            var initiatorId = new UserId(command.InitiatorId);
            
            MemberInRole.ActiveChief(organizationId, initiatorId);

            var articleId = new ArticleId(command.ArticleId);
            var stockId = new StockId(command.StockId);
            var stock = Repository.Get(organizationId, articleId, stockId);

            stock.IncreaseQuantityInInventory(command.Amount);

            Repository.Save(stock);
        }
    }
}
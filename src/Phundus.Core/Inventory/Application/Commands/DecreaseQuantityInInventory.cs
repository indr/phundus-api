namespace Phundus.Core.Inventory.Application.Commands
{
    using Cqrs;
    using Domain.Model.Catalog;
    using Domain.Model.Management;
    using IdentityAndAccess.Domain.Model.Organizations;
    using IdentityAndAccess.Domain.Model.Users;
    using IdentityAndAccess.Queries;

    public class DecreaseQuantityInInventory
    {
        public DecreaseQuantityInInventory(int initiatorId, int organizationId, int articleId, string stockId, int quantity)
        {
            InitiatorId = initiatorId;
            OrganizationId = organizationId;
            ArticleId = articleId;
            StockId = stockId;
            Quantity = quantity;
        }

        public int InitiatorId { get; private set; }
        public int OrganizationId { get; private set; }
        public int ArticleId { get; private set; }
        public string StockId { get; private set; }
        public int Quantity { get; private set; }
    }

    public class DecreaseQuantityInInventoryHandler : IHandleCommand<DecreaseQuantityInInventory>
    {
        public IMemberInRole MemberInRole { get; set; }

        public IStockRepository StockRepository { get; set; }

        public void Handle(DecreaseQuantityInInventory command)
        {
            var organizationId = new OrganizationId(command.OrganizationId);
            var initiatorId = new UserId(command.InitiatorId);

            MemberInRole.ActiveChief(organizationId, initiatorId);

            var articleId = new ArticleId(command.ArticleId);
            var stockId = new StockId(command.StockId);
            var stock = StockRepository.Get(organizationId, articleId, stockId);
            stock.DecreaseQuantityInInventory(command.Quantity);

            StockRepository.Save(stock);
        }
    }
}
namespace Phundus.Core.Inventory.Application.Commands
{
    using Cqrs;
    using Domain.Model.Catalog;
    using Domain.Model.Management;
    using IdentityAndAccess.Domain.Model.Organizations;
    using IdentityAndAccess.Domain.Model.Users;
    using IdentityAndAccess.Queries;

    public class CreateStock
    {
        public CreateStock(UserId initiatorId, OrganizationId organizationId, ArticleId articleId)
        {
            OrganizationId = organizationId;
            InitiatorId = initiatorId;
            ArticleId = articleId;
        }

        public ArticleId ArticleId { get; set; }

        public StockId ResultingStockId { get; set; }

        public OrganizationId OrganizationId { get; set; }

        public UserId InitiatorId { get; set; }
    }

    public class CreateStockHandler : IHandleCommand<CreateStock>
    {
        public IMemberInRole MemberInRole { get; set; }

        public IStockRepository Repository { get; set; }

        public void Handle(CreateStock command)
        {
            MemberInRole.ActiveChief(command.OrganizationId, command.InitiatorId);

            // TODO: Consider creating stocks via factory method on article/product to ensure concurrency?
            var stock = new Stock(Repository.GetNextIdentity(), command.ArticleId);

            Repository.Save(stock);

            command.ResultingStockId = stock.StockId;
        }
    }
}
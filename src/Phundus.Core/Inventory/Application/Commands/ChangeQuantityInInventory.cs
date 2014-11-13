namespace Phundus.Core.Inventory.Application.Commands
{
    using System;
    using Cqrs;
    using Domain.Model.Catalog;
    using Domain.Model.Management;
    using IdentityAndAccess.Domain.Model.Organizations;
    using IdentityAndAccess.Domain.Model.Users;
    using IdentityAndAccess.Queries;

    public class ChangeQuantityInInventory
    {
        public ChangeQuantityInInventory(int initiatorId, int organizationId, int articleId, string stockId, int change, DateTime asOfUtc,
            string comment)
        {
            InitiatorId = initiatorId;
            OrganizationId = organizationId;
            ArticleId = articleId;
            StockId = stockId;
            Change = change;
            AsOfUtc = asOfUtc;
            Comment = comment;
        }

        public int InitiatorId { get; private set; }
        public int OrganizationId { get; private set; }
        public int ArticleId { get; private set; }
        public string StockId { get; private set; }
        public int Change { get; private set; }
        public DateTime AsOfUtc { get; private set; }
        public string Comment { get; set; }
    }

    public class ChangeQuantityInInventoryHandler : IHandleCommand<ChangeQuantityInInventory>
    {
        public IMemberInRole MemberInRole { get; set; }

        public IStockRepository Repository { get; set; }

        public void Handle(ChangeQuantityInInventory command)
        {
            var organizationId = new OrganizationId(command.OrganizationId);
            var initiatorId = new UserId(command.InitiatorId);
            
            MemberInRole.ActiveChief(organizationId, initiatorId);

            var articleId = new ArticleId(command.ArticleId);
            var stockId = new StockId(command.StockId);
            var stock = Repository.Get(organizationId, articleId, stockId);

            stock.ChangeQuantityInInventory(command.Change, command.AsOfUtc, command.Comment);
            
            Repository.Save(stock);
        }
    }
}
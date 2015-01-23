namespace Phundus.Core.Inventory.Application.Commands
{
    using System;
    using Common;
    using Common.Cqrs;
    using Cqrs;
    using Domain.Model.Catalog;
    using Domain.Model.Management;
    using IdentityAndAccess.Domain.Model.Organizations;
    using IdentityAndAccess.Domain.Model.Users;
    using IdentityAndAccess.Queries;

    public class ChangeQuantityInInventory : ICommand
    {
        public ChangeQuantityInInventory(UserId initiatorId, OrganizationId organizationId, ArticleId articleId,
            StockId stockId, int change, DateTime asOfUtc, string comment)
        {
            AssertionConcern.AssertArgumentNotNull(initiatorId, "Initiator id must be provided.");
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided.");
            AssertionConcern.AssertArgumentNotNull(articleId, "Article id must be provided.");
            AssertionConcern.AssertArgumentNotNull(stockId, "Stock id must be provided.");
            AssertionConcern.AssertArgumentNotZero(change, "Change must be greater or less than zero.");
            AssertionConcern.AssertArgumentNotEmpty(asOfUtc, "As of utc must be provided.");

            InitiatorId = initiatorId;
            OrganizationId = organizationId;
            ArticleId = articleId;
            StockId = stockId;
            Change = change;
            AsOfUtc = asOfUtc;
            Comment = comment;
        }

        public UserId InitiatorId { get; private set; }
        public OrganizationId OrganizationId { get; private set; }
        public ArticleId ArticleId { get; private set; }
        public StockId StockId { get; private set; }
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
            MemberInRole.ActiveChief(command.OrganizationId, command.InitiatorId);

            var stock = Repository.Get(command.OrganizationId, command.StockId);

            stock.ChangeQuantityInInventory(command.Change, command.AsOfUtc, command.Comment);

            Repository.Save(stock);
        }
    }
}
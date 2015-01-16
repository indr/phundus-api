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

        public IStockRepository StockRepository { get; set; }

        public IArticleRepository ArticleRepository { get; set; }

        public void Handle(CreateStock command)
        {
            MemberInRole.ActiveChief(command.OrganizationId, command.InitiatorId);

            var article = ArticleRepository.GetById(command.OrganizationId.Id, command.ArticleId.Id);
            var stock = article.CreateStock(StockRepository.GetNextIdentity());

            ArticleRepository.Save(article);
            StockRepository.Save(stock);

            command.ResultingStockId = stock.StockId;
        }
    }
}
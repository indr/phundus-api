namespace Phundus.Core.Inventory.Application.Commands
{
    using Common.Cqrs;
    using Cqrs;
    using Ddd;
    using Domain.Model.Catalog;
    using IdentityAndAccess.Queries;

    public class UpdateArticle : ICommand
    {
        public int InitiatorId { get; set; }
        public int ArticleId { get; set; }

        public string Name { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Specification { get; set; }
        public int GrossStock { get; set; }
        public string Color { get; set; }
    }

    public class UpdateArticleHandler : IHandleCommand<UpdateArticle>
    {
        public IArticleRepository ArticleRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(UpdateArticle command)
        {
            var article = ArticleRepository.GetById(command.ArticleId);

            MemberInRole.ActiveChief(article.OrganizationId, command.InitiatorId);

            article.Caption = command.Name;
            article.Brand = command.Brand;
            article.Price = command.Price;
            article.Description = command.Description;
            article.Specification = command.Specification;
            article.GrossStock = command.GrossStock;
            article.Color = command.Color;

            EventPublisher.Publish(new ArticleUpdated());
        }
    }
}
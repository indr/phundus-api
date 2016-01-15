namespace Phundus.Inventory.Articles.Commands
{
    using Common.Domain.Model;
    using Cqrs;
    using Ddd;
    using IdentityAccess.Queries;
    using Model;
    using Repositories;

    public class UpdateArticle
    {
        public UserGuid InitiatorId { get; set; }
        public int ArticleId { get; set; }

        public string Name { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
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

            MemberInRole.ActiveChief(article.Owner.OwnerId.Id, command.InitiatorId);

            article.Name = command.Name;
            article.Brand = command.Brand;
            article.Price = command.Price;
            article.GrossStock = command.GrossStock;
            article.Color = command.Color;

            EventPublisher.Publish(new ArticleUpdated());
        }
    }
}
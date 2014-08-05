namespace Phundus.Core.Inventory.Commands
{
    using Cqrs;
    using Ddd;
    using IdentityAndAccess.Queries;
    using Model;
    using Repositories;

    public class CreateArticle
    {
        public int InitiatorId { get; set; }
        public int OrganizationId { get; set; }
        public int ArticleId { get; set; }

        public string Name { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Specification { get; set; }
        public int GrossStock { get; set; }
        public string Color { get; set; }
    }

    public class CreateArticleHandler : IHandleCommand<CreateArticle>
    {
        public IArticleRepository ArticleRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(CreateArticle command)
        {
            MemberInRole.ActiveChief(command.OrganizationId, command.InitiatorId);

            var result = new Article(
                command.OrganizationId,
                command.Name);

            result.Brand = command.Brand;
            result.Price = command.Price;
            result.Description = command.Description;
            result.Specification = command.Specification;
            result.GrossStock = command.GrossStock;
            result.Color = command.Color;

            command.ArticleId = ArticleRepository.Add(result);

            EventPublisher.Publish(new ArticleCreated());
        }
    }
}
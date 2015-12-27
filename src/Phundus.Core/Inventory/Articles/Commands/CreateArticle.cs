namespace Phundus.Core.Inventory.Articles.Commands
{
    using Common;
    using Cqrs;
    using Ddd;
    using IdentityAndAccess.Queries;
    using Model;
    using Repositories;
    using Services;

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
        private readonly IArticleRepository _articleRepository;
        private readonly IOwnerService _ownerService;
        private readonly IMemberInRole _memberInRole;

        public CreateArticleHandler(IArticleRepository articleRepository, IOwnerService ownerService, IMemberInRole memberInRole)
        {
            AssertionConcern.AssertArgumentNotNull(articleRepository, "ArticleRepository must be provided.");
            AssertionConcern.AssertArgumentNotNull(ownerService, "OwnerService must be provided.");
            AssertionConcern.AssertArgumentNotNull(memberInRole, "MemberInRole must be provided.");

            _articleRepository = articleRepository;
            _ownerService = ownerService;
            _memberInRole = memberInRole;
        }

        public void Handle(CreateArticle command)
        {
            _memberInRole.ActiveChief(command.OrganizationId, command.InitiatorId);
            var owner = _ownerService.GetByOrganizationId(command.OrganizationId);

            var result = new Article(owner, command.Name);

            result.Brand = command.Brand;
            result.Price = command.Price;
            result.Description = command.Description;
            result.Specification = command.Specification;
            result.GrossStock = command.GrossStock;
            result.Color = command.Color;

            command.ArticleId = _articleRepository.Add(result);

            EventPublisher.Publish(new ArticleCreated());
        }
    }
}
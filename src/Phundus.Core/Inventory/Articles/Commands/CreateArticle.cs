namespace Phundus.Inventory.Articles.Commands
{
    using Common;
    using Common.Domain.Model;
    using Cqrs;
    using Ddd;
    using IdentityAccess.Queries;
    using Model;
    using Repositories;
    using Services;

    public class CreateArticle
    {
        public CreateArticle(UserGuid initiatorId, OwnerId ownerId, StoreId storeId, string name, int amount)
        {
            AssertionConcern.AssertArgumentNotNull(initiatorId, "InitiatorId must be provided.");
            AssertionConcern.AssertArgumentNotNull(ownerId, "OwnerId must be provided.");
            AssertionConcern.AssertArgumentNotNull(storeId, "StoreId must be provided.");
            AssertionConcern.AssertArgumentNotNull(name, "Name must be provided.");

            InitiatorId = initiatorId;
            OwnerId = ownerId;
            StoreId = storeId;
            Name = name;
            Amount = amount;
        }

        public UserGuid InitiatorId { get; protected set; }

        public OwnerId OwnerId { get; protected set; }

        public StoreId StoreId { get; protected set; }

        public string Name { get; protected set; }

        public int Amount { get; protected set; }

        public int ResultingArticleId { get; set; }
    }

    public class CreateArticleHandler : IHandleCommand<CreateArticle>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IMemberInRole _memberInRole;
        private readonly IOwnerService _ownerService;

        public CreateArticleHandler(IArticleRepository articleRepository, IOwnerService ownerService,
            IMemberInRole memberInRole)
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
            _memberInRole.ActiveChief(command.OwnerId, command.InitiatorId);
            var owner = _ownerService.GetById(command.OwnerId);

            var result = new Article(owner, command.StoreId, command.Name, command.Amount);

            command.ResultingArticleId = _articleRepository.Add(result);

            EventPublisher.Publish(new ArticleCreated());
        }
    }
}
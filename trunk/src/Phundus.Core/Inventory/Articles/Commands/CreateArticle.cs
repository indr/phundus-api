namespace Phundus.Core.Inventory.Articles.Commands
{
    using System;
    using Common;
    using Cqrs;
    using Ddd;
    using IdentityAndAccess.Queries;
    using Model;
    using Repositories;
    using Services;
    using Stores.Model;

    public class CreateArticle
    {
        public int InitiatorId { get; set; }
        public Guid OwnerId { get; set; }
        public Guid StoreId { get; set; }
        public string Name { get; set; }
        public int ResultingArticleId { get; set; }
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
            _memberInRole.ActiveChief(command.OwnerId, command.InitiatorId);
            var owner = _ownerService.GetById(command.OwnerId);

            var result = new Article(owner, new StoreId(command.StoreId), command.Name);

            command.ResultingArticleId = _articleRepository.Add(result);

            EventPublisher.Publish(new ArticleCreated());
        }
    }
}
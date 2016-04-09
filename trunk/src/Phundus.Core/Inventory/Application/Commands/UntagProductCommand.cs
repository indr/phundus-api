namespace Phundus.Inventory.Application
{
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model.Articles;
    using Model.Collaborators;

    public class UntagProductCommand : ICommand
    {
        public UntagProductCommand(CurrentUserId initiator, ArticleId productId, string name)
        {
            Initiator = initiator;
            ProductId = productId;
            Name = name;
        }

        public CurrentUserId Initiator { get; protected set; }
        public ArticleId ProductId { get; protected set; }
        public string Name { get; protected set; }
    }

    public class UntagProductCommandHandler : IHandleCommand<UntagProductCommand>
    {
        private readonly ICollaboratorService _collaboratorService;
        private readonly IArticleRepository _articleRepository;

        public UntagProductCommandHandler(ICollaboratorService collaboratorService, IArticleRepository articleRepository)
        {
            _collaboratorService = collaboratorService;
            _articleRepository = articleRepository;
        }

        [Transaction]
        public void Handle(UntagProductCommand command)
        {
            var product = _articleRepository.GetById(command.ProductId);
            var manager = _collaboratorService.Manager(command.Initiator, product.OwnerId);

            product.Untag(manager, command.Name);
        }
    }
}
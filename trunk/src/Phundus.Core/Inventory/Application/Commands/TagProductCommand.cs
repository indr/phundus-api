namespace Phundus.Inventory.Application
{
    using Common.Commanding;
    using Common.Domain.Model;
    using Model.Articles;
    using Model.Collaborators;

    public class TagProductCommand : ICommand
    {
        public TagProductCommand(CurrentUserId initiator, ArticleId productId, string name)
        {
            Initiator = initiator;
            ProductId = productId;
            Name = name;
        }

        public CurrentUserId Initiator { get; protected set; }
        public ArticleId ProductId { get; protected set; }
        public string Name { get; protected set; }
    }

    public class TagProductCommandHandler : IHandleCommand<TagProductCommand>
    {
        private readonly ICollaboratorService _collaboratorService;
        private readonly IArticleRepository _articleRepository;

        public TagProductCommandHandler(ICollaboratorService collaboratorService, IArticleRepository articleRepository)
        {
            _collaboratorService = collaboratorService;
            _articleRepository = articleRepository;
        }

        public void Handle(TagProductCommand command)
        {
            var product = _articleRepository.GetById(command.ProductId);
            var manager = _collaboratorService.Manager(command.Initiator, product.OwnerId);

            product.Tag(manager, command.Name);
        }
    }
}
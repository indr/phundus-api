namespace Phundus.Inventory.Articles.Commands
{
    using Cqrs;
    using IdentityAccess.Queries;
    using Repositories;

    public class UpdateDescription
    {
        public int InitiatorId { get; set; }
        public int ArticleId { get; set; }
        public string Description { get; set; }
    }

    public class UpdateArticleDescriptionHandler : IHandleCommand<UpdateDescription>
    {
        public IArticleRepository ArticleRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(UpdateDescription command)
        {
            var article = ArticleRepository.GetById(command.ArticleId);

            MemberInRole.ActiveChief(article.Owner.OwnerId.Id, command.InitiatorId);

            article.ChangeDescription(command.Description);
        }
    }
}
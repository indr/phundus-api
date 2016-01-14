namespace Phundus.Inventory.Articles.Commands
{
    using Cqrs;
    using IdentityAccess.Queries;
    using Repositories;

    public class RemoveImage
    {
        public int ArticleId { get; set; }
        public string ImageFileName { get; set; }
        public int InitiatorId { get; set; }        
    }

    public class RemoveImageHandler : IHandleCommand<RemoveImage>
    {
        public IArticleRepository ArticleRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(RemoveImage command)
        {
            var article = ArticleRepository.GetById(command.ArticleId);

            MemberInRole.ActiveChief(article.Owner.OwnerId.Id, command.InitiatorId);

            article.RemoveImage(command.ImageFileName);
        }
    }
}
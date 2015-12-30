namespace Phundus.Core.Inventory.Articles.Commands
{
    using Cqrs;
    using Ddd;
    using IdentityAndAccess.Queries;
    using Model;
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

            MemberInRole.ActiveChief(article.Owner.OwnerId.Value, command.InitiatorId);

            article.RemoveImage(command.ImageFileName);
        }
    }
}
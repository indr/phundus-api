namespace Phundus.Inventory.Articles.Commands
{
    using Cqrs;
    using IdentityAccess.Queries;
    using Repositories;

    public class AddImage
    {
        public int ArticleId { get; set; }
        public int? ImageId { get; set; }
        public int InitiatorId { get; set; }

        public long Length { get; set; }
        public string Type { get; set; }
        public string FileName { get; set; }
    }

    public class AddImageHandler : IHandleCommand<AddImage>
    {
        public IArticleRepository ArticleRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(AddImage command)
        {
            var article = ArticleRepository.GetById(command.ArticleId);

            MemberInRole.ActiveChief(article.Owner.OwnerId.Id, command.InitiatorId);

            var image = article.AddImage(command.FileName, command.Type, command.Length);
            command.ImageId = image.Id;
        }
    }
}
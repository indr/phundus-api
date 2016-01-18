namespace Phundus.Inventory.Articles.Commands
{
    using Common.Domain.Model;
    using Cqrs;
    using IdentityAccess.Queries;
    using Repositories;

    public class UpdateSpecification
    {
        public UserId InitiatorId { get; set; }
        public int ArticleId { get; set; }
        public string Specification { get; set; }
    }

    public class UpdateArticleSpecificationHandler : IHandleCommand<UpdateSpecification>
    {
        public IArticleRepository ArticleRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(UpdateSpecification command)
        {
            var article = ArticleRepository.GetById(command.ArticleId);

            MemberInRole.ActiveChief(article.Owner.OwnerId.Id, command.InitiatorId);

            article.ChangeSpecification(command.Specification);
        }
    }
}
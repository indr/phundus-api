namespace Phundus.Core.Inventory.Articles.Commands
{
    using Cqrs;
    using Ddd;
    using IdentityAndAccess.Queries;
    using Model;
    using Repositories;

    public class UpdateSpecification
    {
        public int InitiatorId { get; set; }
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
namespace Phundus.Core.Inventory.Commands
{
    using System.Security;
    using Cqrs;
    using Ddd;
    using IdentityAndAccess.Queries;
    using Model;
    using Repositories;

    public class CreateArticle
    {
        public int InitiatorId { get; set; }
        public int OrganizationId { get; set; }
        public int ArticleId { get; set; }

        public string Name { get; set; }
    }

    public class CreateArticleHandler: IHandleCommand<CreateArticle>
    {
        public IArticleRepository ArticleRepository { get; set; }

        public IMemberInMembershipRoleQueries MemberInMembershipRoleQueries { get; set; }

        public void Handle(CreateArticle command)
        {
            if (!MemberInMembershipRoleQueries.IsActiveChiefIn(command.OrganizationId, command.InitiatorId))
                throw new SecurityException();

            command.ArticleId = ArticleRepository.GetNextIdentifier();

            ArticleRepository.Add(new Article(
                    command.OrganizationId,
                    command.Name
                ));

            EventPublisher.Publish(new ArticleCreated());
        }
    }

    public class ArticleCreated : DomainEvent
    {
        
    }
}
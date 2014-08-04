namespace Phundus.Core.Inventory.Commands
{
    using Cqrs;
    using Ddd;
    using Model;
    using Repositories;

    public class CreateArticle
    {
        public int ArticleId { get; set; }
    }

    public class CreateArticleHandler: IHandleCommand<CreateArticle>
    {
        public IArticleRepository ArticleRepository { get; set; }

        public void Handle(CreateArticle command)
        {
            command.ArticleId = ArticleRepository.GetNextIdentifier();

            ArticleRepository.Add(new Article());

            EventPublisher.Publish(new ArticleCreated());
        }
    }

    public class ArticleCreated : DomainEvent
    {
        
    }
}
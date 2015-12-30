﻿namespace Phundus.Core.Inventory.Articles.Commands
{
    using Cqrs;
    using Ddd;
    using IdentityAndAccess.Queries;
    using Model;
    using Repositories;

    public class DeleteArticle
    {
        public int ArticleId { get; set; }
        public int InitiatorId { get; set; }
    }

    public class DeleteArticleHandler : IHandleCommand<DeleteArticle>
    {
        public IArticleRepository ArticleRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(DeleteArticle command)
        {
            var article = ArticleRepository.GetById(command.ArticleId);

            MemberInRole.ActiveChief(article.Owner.OwnerId.Value, command.InitiatorId);

            ArticleRepository.Remove(article);

            EventPublisher.Publish(new ArticleDeleted());
        }
    }
}
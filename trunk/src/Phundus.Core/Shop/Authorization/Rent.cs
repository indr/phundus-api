namespace Phundus.Shop.Authorization
{
    using System;
    using Common.Domain.Model;
    using IdentityAccess.Queries;
    using Orders.Model;
    using Phundus.Authorization;

    public static class Rent
    {
        public static RentArticle Article(Article article)
        {
            return new RentArticle(article);
        }
    }

    public class RentArticle : IAccessObject
    {
        public RentArticle(Article article)
        {
            if (article == null) throw new ArgumentNullException("article");
            Article = article;
        }

        public Article Article { get; protected set; }
    }

    public class RentArticleAccessObjectHandler : IHandleAccessObject<RentArticle>
    {
        private readonly IMemberInRole _memberInRole;

        public RentArticleAccessObjectHandler(IMemberInRole memberInRole)
        {
            if (memberInRole == null) throw new ArgumentNullException("memberInRole");
            _memberInRole = memberInRole;
        }

        public void Handle(UserId userId, RentArticle accessObject)
        {
            _memberInRole.ActiveMember(accessObject.Article.LessorId.Id, userId);
        }
    }
}
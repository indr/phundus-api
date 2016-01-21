namespace Phundus.Shop.Authorization
{
    using System;
    using Common.Domain.Model;
    using Phundus.Authorization;

    public static class Rent
    {
        public static RentArticle Article(ArticleId articleId)
        {
            return new RentArticle(articleId);
        }
    }

    public class RentArticle : IAccessObject
    {
        public RentArticle(ArticleId articleId)
        {
            if (articleId == null) throw new ArgumentNullException("articleId");
            ArticleId = articleId;
        }

        public ArticleId ArticleId { get; protected set; }
    }

    public class RentArticleAccessObjectHandler : IHandleAccessObject<RentArticle>
    {
        public void Handle(UserId userId, RentArticle accessObject)
        {
            throw new NotImplementedException();
        }
    }
}
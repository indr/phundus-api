namespace Phundus.Shop.Authorization
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
    using Phundus.Authorization;

    //using Phundus.Authorization;

    //public static class Rent
    //{
    //    public static AccessObject Article(ArticleId articleId)
    //    {
    //        return new RentArticle(articleId);
    //    }
    //}

    public class RentArticle : IAccessObject
    {
        public RentArticle(ArticleId articleId)
        {
            if (articleId == null) throw new ArgumentNullException("articleId");
            ArticleId = articleId;
        }

        public ArticleId ArticleId { get; protected set; }
    }

    public class RentArticleAuthorizationHandler : IHandleAuthorization<RentArticle>
    {
        //public void Authorize(UserId userId, RentArticle accessObject)
        //{
        //    throw new NotImplementedException("Rent Article Authorization Handler");
        //}

        public void Handle(RentArticle authorization)
        {
            throw new NotImplementedException();
        }
    }
}
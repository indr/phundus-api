namespace Phundus.Shop.Services
{
    using System;
    using Common.Domain.Model;
    using Orders.Commands;

    public interface IAccessObject {
    }

    public static class Rent
    {
        public static IAccessObject Article(ArticleId articleId)
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

    public interface IAuthorize
    {
        void User(UserId userId, IAccessObject item);
    }
}
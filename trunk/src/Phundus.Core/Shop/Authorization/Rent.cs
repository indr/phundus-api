namespace Phundus.Shop.Authorization
{
    using System;
    using Common;
    using Common.Domain.Model;
    using IdentityAccess.Projections;
    using Model;
    using Phundus.Authorization;
    using Services;

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
        private readonly ILessorService _lessorService;

        public RentArticleAccessObjectHandler(IMemberInRole memberInRole, ILessorService lessorService)
        {
            if (memberInRole == null) throw new ArgumentNullException("memberInRole");
            if (lessorService == null) throw new ArgumentNullException("lessorService");
            _memberInRole = memberInRole;
            _lessorService = lessorService;
        }

        public void Enforce(UserId userId, RentArticle accessObject)
        {
            if (!Test(userId, accessObject))
                throw new AuthorizationException("Du hast keine Berechtigung um diesen Artikel auszuleihen.");
        }

        public bool Test(UserId userId, RentArticle accessObject)
        {
            if (_memberInRole.IsActiveMember(accessObject.Article.LessorId, userId))
                return true;

            if (_lessorService.GetById(accessObject.Article.LessorId).DoesPublicRental)
                return true;

            return false;
        }
    }
}
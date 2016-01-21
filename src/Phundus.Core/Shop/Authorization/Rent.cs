namespace Phundus.Shop.Authorization
{
    using System;
    using System.Security.Authentication;
    using Common;
    using Common.Domain.Model;
    using IdentityAccess.Queries;
    using Orders.Model;
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

        public void Handle(UserId userId, RentArticle accessObject)
        {
            if (_memberInRole.IsActiveMember(accessObject.Article.LessorId.Id, userId))
                return;

            if (_lessorService.GetById(accessObject.Article.LessorId).DoesPublicRental)
                return;

            throw new AuthorizationException("Du hast keine Berechtigung um diesen Artikel auszuleihen.");
        }
    }
}
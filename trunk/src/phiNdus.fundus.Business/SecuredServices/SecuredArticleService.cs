using System;
using System.Collections.Generic;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Business.Paging;
using phiNdus.fundus.Business.Security;
using phiNdus.fundus.Business.Security.Constraints;
using phiNdus.fundus.Business.Services;
using phiNdus.fundus.Domain.Entities;
using User = phiNdus.fundus.Business.Security.Constraints.User;

namespace phiNdus.fundus.Business.SecuredServices
{
    using phiNdus.fundus.Domain;
    using piNuts.phundus.Infrastructure;
    using piNuts.phundus.Infrastructure.Obsolete;

    public class SecuredArticleService : SecuredServiceBase, IArticleService
    {
        #region IArticleService Members

        public ArticleDto GetArticle(string sessionKey, int id)
        {
            return Unsecured.Do<ArticleService, ArticleDto>(svc => svc.GetArticle(id));
        }

        public int CreateArticle(string sessionKey, ArticleDto subject)
        {
            return Secured.With(Session.FromKey(sessionKey))
                //.And(User.InRole(Role.Chief))
                .Do<ArticleService, int>(svc => svc.CreateArticle(subject));
        }

        public void UpdateArticle(string sessionKey, ArticleDto subject)
        {
            Secured.With(Session.FromKey(sessionKey))
                //.And(User.InRole(Role.Chief))
                .Do<ArticleService>(svc => svc.UpdateArticle(subject));
        }

        public IList<FieldDefinitionDto> GetProperties(string sessionKey)
        {
            return GlobalContainer.Resolve<IFieldsService>().GetProperties(sessionKey);
        }

        public ArticleDto[] GetArticles(string sessionKey)
        {
            return Secured.With(Session.FromKey(sessionKey))
                .Do<ArticleService, ArticleDto[]>(svc => svc.GetArticles());
        }

        public void DeleteArticle(string sessionKey, ArticleDto subject)
        {
            Secured.With(Session.FromKey(sessionKey))
                //.And(User.InRole(Role.Chief))
                .Do<ArticleService>(svc => svc.DeleteArticle(subject));
        }

        public IList<AvailabilityDto> GetAvailability(string sessionKey, int id)
        {
            return Unsecured.Do<ArticleService, IList<AvailabilityDto>>(svc => svc.GetAvailability(id));
        }

        public void AddImage(string sessionKey, int articleId, ImageDto subject)
        {
            Secured.With(Session.FromKey(sessionKey))
                //.And(User.InRole(Role.Chief))
                .Do<ArticleService>(svc => svc.AddImage(articleId, subject));
        }

        public void DeleteImage(string sessionKey, int articleId, string imageName)
        {
            Secured.With(Session.FromKey(sessionKey))
                //.And(User.InRole(Role.Chief))
                .Do<ArticleService>(svc => svc.DeleteImage(articleId, imageName));
        }

        public IList<ImageDto> GetImages(string sessionKey, int articleId)
        {
            return Secured.With(Session.FromKey(sessionKey))
                //.And(User.InRole(Role.Chief))
                .Do<ArticleService, IList<ImageDto>>(svc => svc.GetImages(articleId));
        }

        public PagedResult<ArticleDto> FindArticles(string sessionKey, PageRequest pageRequest, string query)
        {
            return Unsecured.Do<ArticleService, PagedResult<ArticleDto>>(svc => svc.FindArticles(pageRequest, query));
        }

        #endregion
    }
}
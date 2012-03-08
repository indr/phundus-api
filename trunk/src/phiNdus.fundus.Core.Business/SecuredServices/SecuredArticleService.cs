using System;
using System.Collections.Generic;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.Paging;
using phiNdus.fundus.Core.Business.Security;
using phiNdus.fundus.Core.Business.Security.Constraints;
using phiNdus.fundus.Core.Business.Services;
using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;
using User = phiNdus.fundus.Core.Business.Security.Constraints.User;

namespace phiNdus.fundus.Core.Business.SecuredServices
{
    public class SecuredArticleService : SecuredServiceBase, IArticleService
    {
        #region IArticleService Members

        public ArticleDto GetArticle(string sessionKey, int id)
        {
            return Secured.With(Session.FromKey(sessionKey))
                .Do<ArticleService, ArticleDto>(svc => svc.GetArticle(id));
        }

        public int CreateArticle(string sessionKey, ArticleDto subject)
        {
            return Secured.With(Session.FromKey(sessionKey))
                .And(User.InRole(Role.Administrator))
                .Do<ArticleService, int>(svc => svc.CreateArticle(subject));
        }

        public void UpdateArticle(string sessionKey, ArticleDto subject)
        {
            Secured.With(Session.FromKey(sessionKey))
                .And(User.InRole(Role.Administrator))
                .Do<ArticleService>(svc => svc.UpdateArticle(subject));
        }

        public IList<FieldDefinitionDto> GetProperties(string sessionKey)
        {
            return IoC.Resolve<IFieldsService>().GetProperties(sessionKey);
        }

        public ArticleDto[] GetArticles(string sessionKey)
        {
            return Secured.With(Session.FromKey(sessionKey))
                .Do<ArticleService, ArticleDto[]>(svc => svc.GetArticles());
        }

        public void DeleteArticle(string sessionKey, ArticleDto subject)
        {
            Secured.With(Session.FromKey(sessionKey))
                .And(User.InRole(Role.Administrator))
                .Do<ArticleService>(svc => svc.DeleteArticle(subject));
        }

        public void AddImage(string sessionKey, int articleId, ImageDto subject)
        {
            Secured.With(Session.FromKey(sessionKey))
                .And(User.InRole(Role.Administrator))
                .Do<ArticleService>(svc => svc.AddImage(articleId, subject));
        }

        public void DeleteImage(string sessionKey, int articleId, string imageName)
        {
            Secured.With(Session.FromKey(sessionKey))
                .And(User.InRole(Role.Administrator))
                .Do<ArticleService>(svc => svc.DeleteImage(articleId, imageName));
        }

        public IList<ImageDto> GetImages(string sessionKey, int articleId)
        {
            return Secured.With(Session.FromKey(sessionKey))
                .And(User.InRole(Role.Administrator))
                .Do<ArticleService, IList<ImageDto>>(svc => svc.GetImages(articleId));
        }

        public PagedResult<ArticleDto> FindArticles(string sessionKey, PageRequest pageRequest, string query)
        {
            return Secured.With(Session.FromKey(sessionKey))
                .Do<ArticleService, PagedResult<ArticleDto>>(svc => svc.FindArticles(pageRequest, query));
        }

        #endregion
    }
}
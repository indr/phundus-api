﻿using System;
using System.Collections.Generic;
using phiNdus.fundus.Core.Business.Dto;
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

        #endregion
    }
}
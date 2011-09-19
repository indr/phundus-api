using System;
using phiNdus.fundus.Core.Business.Assembler;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.Services
{
    public class ArticleService : BaseService
    {
        private static IArticleRepository Articles
        {
            get { return IoC.Resolve<IArticleRepository>(); }
        }

        public virtual void GetArticles()
        {
            Articles.FindAll();
        }

        public virtual ArticleDto GetArticle(int id)
        {
            var article = Articles.Get(id);
            if (article == null)
                return null;
            return ArticleAssembler.CreateDto(article);
        }

        public virtual void CreateArticle()
        {
            using (var uow = UnitOfWork.Start())
            {
                Articles.Save(null);
                uow.TransactionalFlush();
            }
        }

        public virtual void UpdateArticle()
        {
            using (var uow = UnitOfWork.Start())
            {
                Articles.Save(null);
                uow.TransactionalFlush();
            }
        }
    }
}
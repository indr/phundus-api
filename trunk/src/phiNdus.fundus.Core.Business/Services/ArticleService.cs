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
            using(var uow = UnitOfWork.Start())
            {
                var article = Articles.Get(id);
                if (article == null)
                    return null;
                return ArticleAssembler.CreateDto(article);
            }
        }

        public virtual int CreateArticle(ArticleDto subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");

            using (var uow = UnitOfWork.Start())
            {
                var article = ArticleAssembler.CreateDomainObject(subject);
                var id = Articles.Save(article).Id;
                uow.TransactionalFlush();
                return id;
            }
        }

        public virtual void UpdateArticle(ArticleDto subject)
        {
            using (var uow = UnitOfWork.Start())
            {
                var article = ArticleAssembler.UpdateDomainObject(subject);
                Articles.Save(article);
                uow.TransactionalFlush();
            }
        }
    }
}
using System;
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

        public void GetArticles()
        {
            Articles.FindAll();
        }

        public void GetArticle(int id)
        {
            Articles.Get(id);
        }

        public void CreateArticle()
        {
            using (var uow = UnitOfWork.Start())
            {
                Articles.Save(null);
                uow.TransactionalFlush();
            }
        }

        public void UpdateArticle()
        {
            using (var uow = UnitOfWork.Start())
            {
                Articles.Save(null);
                uow.TransactionalFlush();
            }
        }
    }
}
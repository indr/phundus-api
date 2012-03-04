using System;
using System.Collections.Generic;
using System.Linq;
using phiNdus.fundus.Core.Business.Assembler;
using phiNdus.fundus.Core.Business.Dto;
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

        public virtual ArticleDto[] GetArticles()
        {
            using (var uow = UnitOfWork.Start())
            {
                var articles = Articles.FindAll();
                return ArticleAssembler.CreateDtos(articles);
            }
        }

        public virtual ArticleDto GetArticle(int id)
        {
            using (var uow = UnitOfWork.Start())
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

        public virtual void DeleteArticle(ArticleDto subject)
        {
            using (var uow = UnitOfWork.Start())
            {
                var article = ArticleAssembler.UpdateDomainObject(subject);
                Articles.Delete(article);
                uow.TransactionalFlush();
            }
        }

        public void AddImage(int articleId, ImageDto subject)
        {
            using (var uow = UnitOfWork.Start())
            {
                var article = Articles.Get(articleId);
                var assembler = new ImageAssembler();
                article.AddImage(assembler.CreateDomainObject(subject));
                Articles.Update(article);
                uow.TransactionalFlush();
            }
        }

        public void DeleteImage(int articleId, string imageName)
        {
            using (var uow = UnitOfWork.Start())
            {
                var article = Articles.Get(articleId);
                var image = article.Images.Where(i => i.FileName == imageName).FirstOrDefault();
                article.RemoveImage(image);
                Articles.Update(article);
                uow.TransactionalFlush();
            }
        }

        public IList<ImageDto> GetImages(int articleId)
        {
            using (var uow = UnitOfWork.Start())
            {
                var article = Articles.Get(articleId);
                var assembler = new ImageAssembler();
                return assembler.CreateDtos(article.Images);
            }
        }
    }
}
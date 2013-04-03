using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Transform;
using phiNdus.fundus.Business.Assembler;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Business.Paging;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.Domain.Inventory;
using phiNdus.fundus.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Business.Services
{
    using phiNdus.fundus.Domain;
    using Rhino.Commons;
    using piNuts.phundus.Infrastructure;
    using piNuts.phundus.Infrastructure.Rhino;

    public class ArticleService : BaseService
    {
        private static IArticleRepository Articles
        {
            get { return GlobalContainer.Resolve<IArticleRepository>(); }
        }

        public virtual ArticleDto[] GetArticles()
        {
            using (UnitOfWork.Start())
            {
                var articles = Articles.FindAll(SelectedOrganization);
                return new ArticleDtoAssembler().CreateDtos(articles);
            }
        }

        public virtual ArticleDto GetArticle(int id)
        {
            using (UnitOfWork.Start())
            {
                var article = Articles.Get(id);
                if (article == null)
                    return null;
                return new ArticleDtoAssembler().CreateDto(article);
            }
        }

        public virtual int CreateArticle(ArticleDto subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");

            using (var uow = UnitOfWork.Start())
            {
                var article = ArticleDomainAssembler.CreateDomainObject(subject);
                article.Organization = SelectedOrganization;
                var id = Articles.Save(article).Id;
                uow.TransactionalFlush();
                return id;
            }
        }

        public virtual void UpdateArticle(ArticleDto subject)
        {
            using (var uow = UnitOfWork.Start())
            {
                var article = ArticleDomainAssembler.UpdateDomainObject(subject);
                if (article.Organization.Id != SelectedOrganization.Id)
                    throw new InvalidOperationException("Der Artikel gehört nicht der gewählten Organization.");
                Articles.Save(article);
                uow.TransactionalFlush();
            }
        }

        public virtual void DeleteArticle(ArticleDto subject)
        {
            using (var uow = UnitOfWork.Start())
            {
                var article = ArticleDomainAssembler.UpdateDomainObject(subject);
                if (article.Organization.Id != SelectedOrganization.Id)
                    throw new InvalidOperationException("Der Artikel gehört nicht der gewählten Organization.");
                Articles.Delete(article);
                uow.TransactionalFlush();
            }
        }

        public void AddImage(int articleId, ImageDto subject)
        {
            using (var uow = UnitOfWork.Start())
            {
                var article = Articles.Get(articleId);
                if (article.Organization.Id != SelectedOrganization.Id)
                    throw new InvalidOperationException("Der Artikel gehört nicht der gewählten Organization.");
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
                if (article.Organization.Id != SelectedOrganization.Id)
                    throw new InvalidOperationException("Der Artikel gehört nicht der gewählten Organization.");
                var image = article.Images.Where(i => i.FileName == imageName).FirstOrDefault();
                article.RemoveImage(image);
                Articles.Update(article);
                uow.TransactionalFlush();
            }
        }

        public IList<ImageDto> GetImages(int articleId)
        {
            using (UnitOfWork.Start())
            {
                var article = Articles.Get(articleId);
                var assembler = new ImageAssembler();
                return assembler.CreateDtos(article.Images);
            }
        }

        public PagedResult<ArticleDto> FindArticles(PageRequest pageRequest, string query)
        {
            using (UnitOfWork.Start())
            {
                int total;
                var result = Articles.FindMany(query, pageRequest.Index*pageRequest.Size, pageRequest.Size, out total);
                var dtos = new ArticleDtoAssembler().CreateDtos(result).ToList();
                return new PagedResult<ArticleDto>(PageResponse.From(pageRequest, total), dtos);
            }
        }

        public IList<AvailabilityDto> GetAvailability(int id)
        {
            using (UnitOfWork.Start())
            {
                var article = Articles.Get(id);
                var availabilities = new NetStockCalculator(article).From(DateTime.Today).To(DateTime.Today.AddYears(1));
                return  availabilities.Select(each => new AvailabilityDto {Date = each.Date, Amount = each.Amount}).ToList();
            }
        }
    }
}
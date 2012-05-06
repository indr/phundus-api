﻿using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Transform;
using phiNdus.fundus.Business.Assembler;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Business.Paging;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Business.Services
{
    public class ArticleService : BaseService
    {
        private static IArticleRepository Articles
        {
            get { return IoC.Resolve<IArticleRepository>(); }
        }

        public virtual ArticleDto[] GetArticles()
        {
            using (UnitOfWork.Start())
            {
                var articles = Articles.FindAll();
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
                Articles.Save(article);
                uow.TransactionalFlush();
            }
        }

        public virtual void DeleteArticle(ArticleDto subject)
        {
            using (var uow = UnitOfWork.Start())
            {
                var article = ArticleDomainAssembler.UpdateDomainObject(subject);
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
                var grossStock = article.GrossStock;

                var result = UnitOfWork.CurrentSession.CreateSQLQuery(
                    @"select [Date], sum([Amount]) as [Amount] from
(
	select [from] as [Date], 0 - sum(amount) as [Amount] from OrderItem
        inner join [Order] on [Order].Id = [OrderItem].OrderId and ([Order].Status = :pending or [Order].Status = :approved)
        where ArticleId = :id and [From] >= getdate()
        group by [from]
	union
	select dateadd(day, 1, [to]) as [Date], sum(amount) as [Amount] from OrderItem
        inner join [Order] on [Order].Id = [OrderItem].OrderId and ([Order].Status = :pending or [Order].Status = :approved)
        where ArticleId = :id and [To] >= getdate()
        group by [to]
) temp
group by [Date]
order by [Date] asc")
                    .SetParameter("id", id)
                    .SetParameter("pending", OrderStatus.Pending)
                    .SetParameter("approved", OrderStatus.Approved)
                    .SetResultTransformer(Transformers.AliasToBean(typeof (AvailabilityDto)))
                    .List<AvailabilityDto>();

                var amount = grossStock;
                foreach (var each in result)
                {
                    amount = amount + each.Amount;
                    each.Amount = amount;
                }

                if (result.SingleOrDefault(p => p.Date == DateTime.Today) == null)
                    result.Insert(0, new AvailabilityDto {Date = DateTime.Today, Amount = grossStock});

                return result;
            }
        }
    }
}
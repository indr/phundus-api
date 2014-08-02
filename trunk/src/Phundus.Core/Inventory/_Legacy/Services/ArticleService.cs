namespace Phundus.Core.Inventory._Legacy.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Assemblers;
    using Cqrs.Paging;
    using Dtos;
    using IdentityAndAccess.Users.Repositories;
    using Infrastructure;
    using InventoryCtx.Repositories;
    using InventoryCtx.Services;
    using Microsoft.Practices.ServiceLocation;
    using NHibernate;

    public class ArticleService : AppServiceBase, IArticleService
    {
        private static IArticleRepository ArticleRepository
        {
            get { return ServiceLocator.Current.GetInstance<IArticleRepository>(); }
        }


        public IUserRepository Users { get; set; }
        public IPropertyService PropertyService { get; set; }

        public Func<ISession> Session { get; set; }

        #region IArticleService Members

        public IList<FieldDefinitionDto> GetProperties()
        {
            return PropertyService.GetProperties();
        }

        public virtual int CreateArticle(ArticleDto subject, int organizationId)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");

            var article = ArticleDomainAssembler.CreateDomainObject(subject);
            var user = Users.FindByEmail(Identity.Name);
            article.OrganizationId = organizationId;
            var id = ArticleRepository.Add(article).Id;
            return id;
        }
         

        public virtual void UpdateArticle(ArticleDto subject, int organizationId)
        {
            var user = Users.FindByEmail(Identity.Name);
            var article = ArticleDomainAssembler.UpdateDomainObject(subject);
            if (article.OrganizationId != organizationId)
                throw new InvalidOperationException("Der Artikel gehört nicht der gewählten Organization.");
            ArticleRepository.Add(article);
        }
       
        public void AddImage(int articleId, ImageDto subject, int organizationId)
        {
            var user = Users.FindByEmail(Identity.Name);
            var article = ArticleRepository.ById(articleId);
            if (article.OrganizationId != organizationId)
                throw new InvalidOperationException("Der Artikel gehört nicht der gewählten Organization.");
            var assembler = new ImageAssembler();
            article.AddImage(assembler.CreateDomainObject(subject));
            ArticleRepository.Add(article);
        }

        public void DeleteImage(int articleId, string imageName, int organizationId)
        {
            var user = Users.FindByEmail(Identity.Name);
            var article = ArticleRepository.ById(articleId);
            if (article.OrganizationId != organizationId)
                throw new InvalidOperationException("Der Artikel gehört nicht der gewählten Organization.");
            var image = article.Images.Where(i => i.FileName == imageName).FirstOrDefault();
            article.RemoveImage(image);
            ArticleRepository.Add(article);
        }

        public IList<ImageDto> GetImages(int articleId)
        {
            var article = ArticleRepository.ById(articleId);
            var assembler = new ImageAssembler();
            return assembler.CreateDtos(article.Images);
        }

        public IList<AvailabilityDto> GetAvailability(int id)
        {
            var article = ArticleRepository.ById(id);
            var availabilities = new NetStockCalculator(article, Session())
                .From(DateTime.Today).To(DateTime.Today.AddYears(1));
            return
                availabilities.Select(each => new AvailabilityDto {Date = each.Date, Amount = each.Amount}).ToList();
        }

        #endregion
    }
}
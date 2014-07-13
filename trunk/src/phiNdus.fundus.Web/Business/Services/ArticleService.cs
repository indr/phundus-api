namespace phiNdus.fundus.Web.Business.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Practices.ServiceLocation;
    using NHibernate;
    using phiNdus.fundus.Business.Paging;
    using phiNdus.fundus.Domain.Inventory;
    using phiNdus.fundus.Domain.Repositories;
    using phiNdus.fundus.Web.Business.Assembler;
    using phiNdus.fundus.Web.Business.Dto;
    using piNuts.phundus.Infrastructure;

    public class ArticleService : BaseService, IArticleService
    {
        private static IArticleRepository Articles
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

        public virtual ArticleDto[] GetArticles()
        {
            var user = Users.FindByEmail(Identity.Name);
            var articles = Articles.FindAll(user.SelectedOrganization);
            return new ArticleDtoAssembler().CreateDtos(articles);
        }

        public virtual ArticleDto GetArticle(int id)
        {
            var article = Articles.ById(id);
            if (article == null)
                return null;
            return new ArticleDtoAssembler().CreateDto(article);
        }

        public virtual int CreateArticle(ArticleDto subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");

            var article = ArticleDomainAssembler.CreateDomainObject(subject);
            var user = Users.FindByEmail(Identity.Name);
            article.Organization = user.SelectedOrganization;
            var id = Articles.Add(article).Id;
            return id;
        }


        public virtual void UpdateArticle(ArticleDto subject)
        {
            var user = Users.FindByEmail(Identity.Name);
            var article = ArticleDomainAssembler.UpdateDomainObject(subject);
            if (article.Organization.Id != user.SelectedOrganization.Id)
                throw new InvalidOperationException("Der Artikel gehört nicht der gewählten Organization.");
            Articles.Add(article);
        }

        public virtual void DeleteArticle(ArticleDto subject)
        {
            var user = Users.FindByEmail(Identity.Name);
            var article = ArticleDomainAssembler.UpdateDomainObject(subject);
            if (article.Organization.Id != user.SelectedOrganization.Id)
                throw new InvalidOperationException("Der Artikel gehört nicht der gewählten Organization.");
            Articles.Remove(article);
        }

        public void AddImage(int articleId, ImageDto subject)
        {
            var user = Users.FindByEmail(Identity.Name);
            var article = Articles.ById(articleId);
            if (article.Organization.Id != user.SelectedOrganization.Id)
                throw new InvalidOperationException("Der Artikel gehört nicht der gewählten Organization.");
            var assembler = new ImageAssembler();
            article.AddImage(assembler.CreateDomainObject(subject));
            Articles.Add(article);
        }

        public void DeleteImage(int articleId, string imageName)
        {
            var user = Users.FindByEmail(Identity.Name);
            var article = Articles.ById(articleId);
            if (article.Organization.Id != user.SelectedOrganization.Id)
                throw new InvalidOperationException("Der Artikel gehört nicht der gewählten Organization.");
            var image = article.Images.Where(i => i.FileName == imageName).FirstOrDefault();
            article.RemoveImage(image);
            Articles.Add(article);
        }

        public IList<ImageDto> GetImages(int articleId)
        {
            var article = Articles.ById(articleId);
            var assembler = new ImageAssembler();
            return assembler.CreateDtos(article.Images);
        }

        public PagedResult<ArticleDto> FindArticles(PageRequest pageRequest, string query, int? organization)
        {
            int total;
            var result = Articles.FindMany(query, organization, pageRequest.Index*pageRequest.Size, pageRequest.Size,
                                           out total);
            var dtos = new ArticleDtoAssembler().CreateDtos(result).ToList();
            return new PagedResult<ArticleDto>(PageResponse.From(pageRequest, total), dtos);
        }


        public IList<AvailabilityDto> GetAvailability(int id)
        {
            var article = Articles.ById(id);
            var availabilities = new NetStockCalculator(article, Session())
                .From(DateTime.Today).To(DateTime.Today.AddYears(1));
            return
                availabilities.Select(each => new AvailabilityDto {Date = each.Date, Amount = each.Amount}).ToList();
        }

        #endregion
    }
}
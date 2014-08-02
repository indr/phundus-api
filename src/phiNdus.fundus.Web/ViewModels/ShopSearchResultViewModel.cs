namespace phiNdus.fundus.Web.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.Practices.ServiceLocation;
    using Phundus.Core.Cqrs.Paging;
    using Phundus.Core.IdentityAndAccess.Organizations.Model;
    using Phundus.Core.Inventory._Legacy.Services;

    public class ShopSearchResultViewModel : ViewModelBase
    {
        private readonly ICollection<Organization> _organizations;

        public ShopSearchResultViewModel(string queryString, int? queryOrganizationId, int page, int rowsPerPage,
            ICollection<Organization> organizations)
        {
            Query = queryString;
            QueryOrganizationId = queryOrganizationId;
            RowsPerPage = rowsPerPage;
            Articles = new List<ArticleViewModel>();
            _organizations = organizations;

            Search(Query, QueryOrganizationId, page);
        }

        protected int RowsPerPage { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Query { get; protected set; }

        public PageSelectorViewModel PageSelectorModel { get; set; }
        public IList<ArticleViewModel> Articles { get; private set; }

        public IEnumerable<Organization> Organizations
        {
            get { return _organizations; }
        }


        public int? QueryOrganizationId { get; set; }

        protected IArticleService ArticleService
        {
            get { return ServiceLocator.Current.GetInstance<IArticleService>(); }
        }

        private void Search(string query, int? organization, int page)
        {
            var fieldDefinitions = ArticleService.GetProperties();
            var queryResult = ServiceLocator.Current.GetInstance<IArticleService>().FindArticles(
                new PageRequest {Index = page - 1, Size = RowsPerPage}, query, organization);
            PageSelectorModel = new PageSelectorViewModel(queryResult.Pages);
            foreach (var each in queryResult.Items)
            {
                Articles.Add(new ArticleViewModel(each, fieldDefinitions));
            }
        }
    }
}
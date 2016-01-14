namespace phiNdus.fundus.Web.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using Phundus.Cqrs.Paging;
    using Phundus.IdentityAccess.Queries.ReadModels;
    using Phundus.Shop.Queries;

    public class ShopSearchResultViewModel : ViewModelBase
    {
        private readonly IEnumerable<OrganizationDto> _organizations;
        private IEnumerable<ShopArticleSearchResultDto> _articles = new Collection<ShopArticleSearchResultDto>();

        public ShopSearchResultViewModel(string queryString, Guid? queryOrganizationId, int page, int rowsPerPage,
            IEnumerable<OrganizationDto> organizations, IShopArticleQueries shopArticleQueries)
        {
            Query = queryString;
            QueryOrganizationId = queryOrganizationId;
            RowsPerPage = rowsPerPage;

            _organizations = organizations;

            Search(Query, QueryOrganizationId, page, shopArticleQueries);
        }

        protected int RowsPerPage { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Query { get; protected set; }

        public PageSelectorViewModel PageSelectorModel { get; set; }

        public IEnumerable<ShopArticleSearchResultDto> Articles
        {
            get { return _articles; }
            set { _articles = value; }
        }

        public IEnumerable<OrganizationDto> Organizations
        {
            get { return _organizations; }
        }

        public Guid? QueryOrganizationId { get; set; }

        private void Search(string query, Guid? organization, int page, IShopArticleQueries shopArticleQueries)
        {
            var queryResult = shopArticleQueries.FindArticles(
                new PageRequest {Index = page - 1, Size = RowsPerPage}, query, organization);
            PageSelectorModel = new PageSelectorViewModel(queryResult.Pages);
            Articles = queryResult.Items;
        }
    }
}
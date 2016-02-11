namespace phiNdus.fundus.Web.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Bootstrap;
    using Phundus.Cqrs.Paging;
    using Phundus.Integration.IdentityAccess;
    using Phundus.Shop.Projections;
    using Phundus.Shop.Queries;

    public class ShopSearchResultViewModel
    {
        private readonly IEnumerable<IOrganization> _organizations;
        private IEnumerable<ResultItemsProjectionRow> _items = new List<ResultItemsProjectionRow>();

        public ShopSearchResultViewModel(string queryString, Guid? queryOrganizationId, int page, int rowsPerPage,
            IEnumerable<IOrganization> organizations, IShopArticleQueries shopArticleQueries)
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

        public IEnumerable<ResultItemsProjectionRow> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public IEnumerable<IOrganization> Organizations
        {
            get { return _organizations; }
        }

        public Guid? QueryOrganizationId { get; set; }

        private void Search(string query, Guid? organization, int page, IShopArticleQueries shopArticleQueries)
        {
            var queryResult = shopArticleQueries.FindArticles(
                new PageRequest {Index = page - 1, Size = RowsPerPage}, query, organization);
            PageSelectorModel = new PageSelectorViewModel(queryResult.Pages);
            //queryResult.Items.ForEach(article => article.PreviewImageFileName = GenerateImageFileName(article));
            Items = queryResult.Items;
        }

        private string GenerateImageFileName(ResultItemsProjectionRow article)
        {
            return String.Format(@"~\Content\Images\Articles\{0}\{1}", article.ItemShortId, article.PreviewImageFileName);
        }
    }
}
using System;
using System.Collections.Generic;
using phiNdus.fundus.Core.Business.Paging;
using phiNdus.fundus.Core.Business.SecuredServices;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Web.ViewModels
{
    public class ShopSearchResultViewModel : ViewModelBase
    {
        public ShopSearchResultViewModel()
            : this(null, 1, 8)
        {
        }

        public ShopSearchResultViewModel(string query, int page, int rowsPerPage)
        {
            Query = query;
            RowsPerPage = rowsPerPage;       
            Articles = new List<ArticleViewModel>();

            Search(Query, page);
        }

        protected int RowsPerPage { get; set; }

        public string Query { get; protected set; }
        public PageSelectorViewModel PageSelectorModel { get; set; }
        public IList<ArticleViewModel> Articles { get; private set; }
        

        protected IArticleService ArticleService
        {
            get { return IoC.Resolve<IArticleService>(); }
        }

        private void Search(string query, int page)
        {
            var fieldDefinitions = ArticleService.GetProperties(SessionId);
            var queryResult = IoC.Resolve<IArticleService>().FindArticles(SessionId,
                    new PageRequest { Index = page - 1, Size = RowsPerPage }, query);
            PageSelectorModel = new PageSelectorViewModel(queryResult.Pages);
            foreach (var each in queryResult.Items)
            {
                Articles.Add(new ArticleViewModel(each, fieldDefinitions));
            }
        }
    }
}
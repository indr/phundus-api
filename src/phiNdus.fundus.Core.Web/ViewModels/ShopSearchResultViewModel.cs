using System.Collections.Generic;
using phiNdus.fundus.Core.Business.SecuredServices;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Web.ViewModels
{
    public class ShopSearchResultViewModel : ViewModelBase
    {
        public ShopSearchResultViewModel()
            : this(null)
        {
        }

        public ShopSearchResultViewModel(string query)
        {
            Articles = new List<ArticleViewModel>();
            Search(query);
        }

        protected IArticleService ArticleService
        {
            get { return IoC.Resolve<IArticleService>(); }
        }

        public IList<ArticleViewModel> Articles { get; private set; }

        private void Search(string query)
        {
            var fieldDefinitions = ArticleService.GetProperties(SessionId);
            foreach (var each in IoC.Resolve<IArticleService>().FindArticles(SessionId, query))
            {
                Articles.Add(new ArticleViewModel(each, fieldDefinitions));
            }
        }
    }
}
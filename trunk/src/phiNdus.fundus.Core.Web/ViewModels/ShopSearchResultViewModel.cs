using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.SecuredServices;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Web.ViewModels
{
    public class ShopSearchResultViewModel : ViewModelBase
    {
        public ShopSearchResultViewModel(string query)
        {
            Articles = new List<ArticleViewModel>();
            Search(query);
        }

        private void Search(string query)
        {

            foreach (var each in IoC.Resolve<IArticleService>().FindArticles(SessionId, query))
            {
                Articles.Add(new ArticleViewModel(each));
            }
        }

        public IList<ArticleViewModel> Articles { get; private set; }
    }
}
namespace phiNdus.fundus.Web.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Phundus.Core.Inventory.Queries;

    public class ArticlesViewModel
    {
        private readonly IEnumerable<ArticleViewModel> _articles;


        public ArticlesViewModel(
            IEnumerable<ArticleDto> articles
            )
        {
            _articles = articles.Select(each => new ArticleViewModel(each));
        }


        public IEnumerable<ArticleViewModel> Articles
        {
            get { return _articles; }
        }
    }
}
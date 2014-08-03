namespace phiNdus.fundus.Web.ViewModels
{
    using System.Collections.Generic;
    using Phundus.Core.Inventory.Queries;

    public class ArticlesTableViewModel
    {
        private readonly IEnumerable<ArticleDto> _articles = new List<ArticleDto>();

        public ArticlesTableViewModel(IEnumerable<ArticleDto> articles)
        {
            _articles = articles;
        }

        public IEnumerable<ArticleDto> Articles
        {
            get { return _articles; }
        }
    }
}
using System.Collections.Generic;
using System.Linq;

namespace phiNdus.fundus.Web.ViewModels
{
    using Phundus.Core.Inventory._Legacy.Dtos;

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
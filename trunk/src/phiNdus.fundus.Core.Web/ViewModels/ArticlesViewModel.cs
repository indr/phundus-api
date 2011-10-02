using System.Collections.Generic;
using System.Linq;
using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Web.ViewModels
{
    public class ArticlesViewModel
    {
        private readonly IEnumerable<ArticleViewModel> _articles;
        private readonly IList<PropertyDto> _propertyDefinitions;

        public ArticlesViewModel(
            IEnumerable<ArticleDto> articles,
            IList<PropertyDto> propertyDefinitions
            )
        {
            _articles = articles.Select(each => new ArticleViewModel(each, propertyDefinitions));
            _propertyDefinitions = propertyDefinitions;
        }

        protected IList<PropertyDto> PropertyDefinitions
        {
            get { return _propertyDefinitions; }
        }

        public IEnumerable<ArticleViewModel> Articles
        {
            get { return _articles; }
        }
    }
}
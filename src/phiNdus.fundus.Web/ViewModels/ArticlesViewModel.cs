using System.Collections.Generic;
using System.Linq;

namespace phiNdus.fundus.Web.ViewModels
{
    using Phundus.Core.Inventory._Legacy.Dtos;

    public class ArticlesViewModel
    {
        private readonly IEnumerable<ArticleViewModel> _articles;
        private readonly IList<FieldDefinitionDto> _propertyDefinitions;

        public ArticlesViewModel(
            IEnumerable<ArticleDto> articles,
            IList<FieldDefinitionDto> propertyDefinitions
            )
        {
            _articles = articles.Select(each => new ArticleViewModel(each, propertyDefinitions));
            _propertyDefinitions = propertyDefinitions;
        }

        protected IList<FieldDefinitionDto> PropertyDefinitions
        {
            get { return _propertyDefinitions; }
        }

        public IEnumerable<ArticleViewModel> Articles
        {
            get { return _articles; }
        }
    }
}
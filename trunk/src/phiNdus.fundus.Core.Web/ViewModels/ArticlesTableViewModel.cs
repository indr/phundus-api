using System.Collections.Generic;
using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Web.ViewModels
{
    public class ArticlesTableViewModel
    {
        private readonly IEnumerable<ArticleDto> _articles = new List<ArticleDto>();
        private readonly IList<PropertyDto> _propertyDefinitions = new List<PropertyDto>();
        private IEnumerable<PropertyDto> _headings;

        public ArticlesTableViewModel(IEnumerable<ArticleDto> articles, IList<PropertyDto> propertyDefinitions)
        {
            _articles = articles;
            _propertyDefinitions = propertyDefinitions;
        }
        
        public IEnumerable<ArticleDto> Articles
        {
            get { return _articles; }
        }

        public IEnumerable<PropertyDto> Headings
        {
            get
            {
                if (_headings == null)
                    _headings = ComputeHeadings();
                return _headings;
            }
        }

        private IEnumerable<PropertyDto> ComputeHeadings()
        {
            return _propertyDefinitions;
        }
    }
}
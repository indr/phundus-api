using System.Collections.Generic;
using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Web.ViewModels
{
    public class ArticlesTableViewModel
    {
        private readonly IEnumerable<ArticleDto> _articles = new List<ArticleDto>();
        private readonly IList<FieldDefinitionDto> _propertyDefinitions = new List<FieldDefinitionDto>();
        private IEnumerable<FieldDefinitionDto> _headings;

        public ArticlesTableViewModel(IEnumerable<ArticleDto> articles, IList<FieldDefinitionDto> propertyDefinitions)
        {
            _articles = articles;
            _propertyDefinitions = propertyDefinitions;
        }
        
        public IEnumerable<ArticleDto> Articles
        {
            get { return _articles; }
        }

        public IEnumerable<FieldDefinitionDto> Headings
        {
            get
            {
                if (_headings == null)
                    _headings = ComputeHeadings();
                return _headings;
            }
        }

        private IEnumerable<FieldDefinitionDto> ComputeHeadings()
        {
            return _propertyDefinitions;
        }
    }
}
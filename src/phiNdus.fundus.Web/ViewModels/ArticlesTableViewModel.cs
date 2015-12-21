namespace phiNdus.fundus.Web.ViewModels
{
    using System.Collections.Generic;
    using Phundus.Core.Inventory.Queries;

    public class ArticlesTableViewModel
    {
        private readonly IEnumerable<ArticleDto> _articles = new List<ArticleDto>();
        private int _organizationId;

        public ArticlesTableViewModel(int organizationId, IEnumerable<ArticleDto> articles)
        {
            _organizationId = organizationId;
            _articles = articles;
        }

        public int OrganizationId
        {
            get { return _organizationId; }
            private set { _organizationId = value; }
        }

        public IEnumerable<ArticleDto> Articles
        {
            get { return _articles; }
        }
    }
}
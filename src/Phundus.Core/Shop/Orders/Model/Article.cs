namespace Phundus.Core.Shop.Orders.Model
{
    using System.Collections.Generic;
    using Common.Domain.Model;

    public class Article : ValueObject
    {
        private int _articleId;
        private string _caption;
        private int _organizationId;
        private decimal _price;

        public Article(int articleId, int organizationId, string name, decimal pricePerWeek)
        {
            _articleId = articleId;
            _organizationId = organizationId;
            _caption = name;
            _price = pricePerWeek;
        }

        protected Article()
        {
        }

        public virtual int ArticleId
        {
            get { return _articleId; }
            protected set { _articleId = value; }
        }

        public virtual int OrganizationId
        {
            get { return _organizationId; }
            protected set { _organizationId = value; }
        }

        public virtual string Caption
        {
            get { return _caption; }
            protected set { _caption = value; }
        }

        public virtual decimal Price
        {
            get { return _price; }
            protected set { _price = value; }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return ArticleId;
        }
    }
}
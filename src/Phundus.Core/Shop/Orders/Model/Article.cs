namespace Phundus.Shop.Orders.Model
{
    using System.Collections.Generic;
    using Common;
    using Common.Domain.Model;

    public class Article : ValueObject
    {
        private int _articleId;
        private string _caption;
        private Owner _owner;
        private decimal _price;

        public Article(int articleId, Owner owner, string name, decimal pricePerWeek)
        {
            AssertionConcern.AssertArgumentNotNull(owner, "Owner must be provided.");

            _articleId = articleId;
            _owner = owner;
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

        public virtual Owner Owner
        {
            get { return _owner; }
            protected set { _owner = value; }
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
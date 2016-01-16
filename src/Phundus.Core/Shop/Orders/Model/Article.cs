namespace Phundus.Shop.Orders.Model
{
    using System.Collections.Generic;
    using Common;
    using Common.Domain.Model;

    public class Article : ValueObject
    {
        private string _caption;
        private int _id;
        private Owner _owner;
        private decimal _price;

        public Article(int id, Owner owner, string name, decimal pricePerWeek)
        {
            AssertionConcern.AssertArgumentNotNull(owner, "Owner must be provided.");

            _id = id;
            _owner = owner;
            _caption = name;
            _price = pricePerWeek;
        }

        protected Article()
        {
        }

        public virtual int Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual ArticleId ArticleId
        {
            get { return new ArticleId(Id); }
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
            yield return Id;
        }
    }
}
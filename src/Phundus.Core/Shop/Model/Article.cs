namespace Phundus.Shop.Orders.Model
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;

    public class Article : ValueObject
    {
        private string _caption;
        private int _id;
        private Owner _owner;
        private decimal _publicPrice;
        private ArticleId _articleId;

        public Article(int id, ArticleId articleId, Owner owner, string name, decimal publicPrice)
        {
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (owner == null) throw new ArgumentNullException("owner");

            _id = id;
            _articleId = articleId;
            _owner = owner;
            _caption = name;
            _publicPrice = publicPrice;
        }

        protected Article()
        {
        }

        public virtual ArticleId ArticleId
        {
            get { return _articleId; }
            protected set { _articleId = value; }
        }

        public virtual int Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual ArticleShortId ArticleShortId
        {
            get { return new ArticleShortId(Id); }
        }

        public virtual Owner Owner
        {
            get { return _owner; }
            protected set { _owner = value; }
        }

        public virtual LessorId LessorId
        {
            get { return new LessorId(_owner.OwnerId.Id); }
        }

        public virtual string Caption
        {
            get { return _caption; }
            protected set { _caption = value; }
        }

        public virtual decimal Price
        {
            get { return _publicPrice; }
            protected set { _publicPrice = value; }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
        }
    }
}
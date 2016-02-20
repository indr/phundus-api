namespace Phundus.Shop.Orders.Model
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;

    public class Article : ValueObject
    {
        private ArticleId _articleId;
        private ArticleShortId _articleShortId;
        private string _caption;
        private Owner _owner;
        private decimal _publicPrice;

        public Article(ArticleShortId articleShortId, ArticleId articleId, Owner owner, string name, decimal publicPrice)
        {
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (owner == null) throw new ArgumentNullException("owner");

            _articleShortId = articleShortId;
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

        public virtual ArticleShortId ArticleShortId
        {
            get { return _articleShortId; }
            protected set { _articleShortId = value; }
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
            yield return ArticleId;
        }
    }
}
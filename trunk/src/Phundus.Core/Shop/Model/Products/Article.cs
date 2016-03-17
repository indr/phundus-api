namespace Phundus.Shop.Model.Products
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;

    public class Article : ValueObject
    {
        private ArticleId _articleId;
        private ArticleShortId _articleShortId;
        private Lessor _lessor;        
        private string _name;
        private decimal _price;

        public Article(ArticleShortId articleShortId, ArticleId articleId, Lessor lessor, StoreId storeId, string name,
            decimal price)
        {
            if (articleShortId == null) throw new ArgumentNullException("articleShortId");
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (lessor == null) throw new ArgumentNullException("lessor");
            if (storeId == null) throw new ArgumentNullException("storeId");
            _articleShortId = articleShortId;
            _articleId = articleId;
            _lessor = lessor;
            StoreId = storeId;
            _name = name;
            _price = price;
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

        public virtual Lessor Lessor
        {
            get { return _lessor; }
            protected set { _lessor = value; }
        }

        public virtual LessorId LessorId
        {
            get { return Lessor.LessorId; }
        }

        public StoreId StoreId { get; private set; }

        public virtual string Name
        {
            get { return _name; }
            protected set { _name = value; }
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
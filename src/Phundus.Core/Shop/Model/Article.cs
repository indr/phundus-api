namespace Phundus.Shop.Model
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Orders.Model;

    public class Article : ValueObject
    {
        private ArticleId _articleId;
        private ArticleShortId _articleShortId;
        private string _name;
        private Lessor _lessor;
        private decimal _publicPrice;

        public Article(ArticleShortId articleShortId, ArticleId articleId, Lessor lessor, string name, decimal publicPrice)
        {
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (lessor == null) throw new ArgumentNullException("lessor");

            _articleShortId = articleShortId;
            _articleId = articleId;
            _lessor = lessor;
            _name = name;
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

        public virtual Lessor Lessor
        {
            get { return _lessor; }
            protected set { _lessor = value; }
        }

        public virtual LessorId LessorId
        {
            get { return Lessor.LessorId; }
        }

        public virtual string Name
        {
            get { return _name; }
            protected set { _name = value; }
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
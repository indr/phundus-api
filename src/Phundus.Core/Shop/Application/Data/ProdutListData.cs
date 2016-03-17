namespace Phundus.Shop.Application
{
    using System;
    using System.Collections.Generic;

    public class ProductListData
    {
        private ICollection<ProductPopularityData> _popularities = new List<ProductPopularityData>();

        public virtual Guid ArticleId { get; set; }
        public virtual int ArticleShortId { get; set; }

        public virtual Guid LessorId { get; set; }
        public virtual LessorType LessorType { get; set; }
        public virtual string LessorName { get; set; }
        public virtual string LessorUrl { get; set; }

        public virtual Guid StoreId { get; set; }
        public virtual string StoreName { get; set; }
        public virtual string StoreUrl { get; set; }

        public virtual DateTime CreatedAtUtc { get; set; }
        public virtual string Name { get; set; }
        public virtual decimal PublicPrice { get; set; }
        public virtual decimal? MemberPrice { get; set; }
        public virtual string PreviewImageFileName { get; set; }

        public virtual ICollection<ProductPopularityData> Popularities
        {
            get { return _popularities; }
            protected set { _popularities = value; }
        }
    }

    public class ProductPopularityData
    {
        private Guid _articleId;
        private int _month;
        private ProductListData _productList;

        public ProductPopularityData(ProductListData productList, Guid articleId, int month)
        {
            _productList = productList;
            _articleId = articleId;
            _month = month;
        }

        protected ProductPopularityData()
        {
        }

        public virtual Guid RowId { get; protected set; }

        public virtual ProductListData ProductList
        {
            get { return _productList; }
            protected set { _productList = value; }
        }

        public virtual Guid ArticleId
        {
            get { return _articleId; }
            protected set { _articleId = value; }
        }

        public virtual int Month
        {
            get { return _month; }
            protected set { _month = value; }
        }

        public virtual int Value { get; set; }
    }
}
namespace Phundus.Shop.Application
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ProductListData
    {
        private ICollection<ProductListPopularityData> _popularities = new List<ProductListPopularityData>();
        private ISet<string> _tags = new HashSet<string>();

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

        public virtual ICollection<ProductListPopularityData> Popularities
        {
            get { return _popularities; }
            protected set { _popularities = value; }
        }

        public virtual ICollection<string> Tags
        {
            get { return _tags; }            
        }

        public virtual string TagsAsString
        {
            get { return String.Join(" ", Tags); }
            protected set
            {
                // Noop
            }
        }
    }

    public class ProductListPopularityData
    {
        private Guid _articleId;
        private int _month;
        private ProductListData _productList;

        public ProductListPopularityData(ProductListData productList, Guid articleId, int month)
        {
            _productList = productList;
            _articleId = articleId;
            _month = month;
        }

        protected ProductListPopularityData()
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
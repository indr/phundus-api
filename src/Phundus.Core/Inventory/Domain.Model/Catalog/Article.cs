namespace Phundus.Core.Inventory.Domain.Model.Catalog
{
    using System;
    using System.Linq;
    using IdentityAndAccess.Domain.Model.Organizations;
    using Iesi.Collections.Generic;
    using Management;

    public class Article
    {
        private string _caption;
        private DateTime _createDate = DateTime.Now;
        private ISet<Image> _images = new HashedSet<Image>();
        private int _organizationId;
        private int _id;

        protected Article()
        {
        }

        public Article(int organizationId, string name)
        {
            _organizationId = organizationId;
            _caption = name;
        }

        public Article(ArticleId articleId, int organizationId, string name)
        {
            _id = articleId.Id;
            _organizationId = organizationId;
            _caption = name;
        }

        public virtual int Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual int Version { get; protected set; }

        public virtual int OrganizationId
        {
            get { return _organizationId; }
            protected set { _organizationId = value; }
        }

        public virtual ISet<Image> Images
        {
            get { return _images; }
            set { _images = value; }
        }

        public virtual DateTime CreateDate
        {
            get { return _createDate; }
            protected set { _createDate = value; }
        }

        public virtual string Caption
        {
            get { return _caption; }
            set { _caption = value; }
        }

        public virtual string Brand { get; set; }

        public virtual decimal Price { get; set; }

        public virtual int GrossStock { get; set; }

        public virtual string Description { get; set; }

        public virtual string Specification { get; set; }

        public virtual string Color { get; set; }

        public virtual string StockId { get; set; }

        public virtual bool RemoveImage(Image image)
        {
            var result = Images.Remove(image);
            image.Article = null;
            return result;
        }

        public virtual Image AddImage(string fileName, string type, long length)
        {
            var image = new Image
            {
                FileName = fileName,
                Length = length,
                Type = type,
                Article = this
            };
            Images.Add(image);
            return image;
        }

        public virtual void RemoveImage(string fileName)
        {
            var image = Images.FirstOrDefault(p => p.FileName == fileName);
            if (image == null)
                return;
            Images.Remove(image);
            image.Article = null;
        }

        public virtual Stock CreateStock(StockId stockId)
        {
            if (StockId != null)
                throw new InvalidOperationException("Article has already a stock. Only one stock is currently supported.");

            if (Equals(stockId, Management.StockId.Default))
                throw new InvalidOperationException("Stock id must not be the default stock id.");

            StockId = stockId.Id;
            return new Stock(new OrganizationId(OrganizationId), new ArticleId(Id), stockId);
        }
    }
}
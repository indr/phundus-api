namespace Phundus.Inventory.Articles.Model
{
    using System;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using Ddd;
    using Iesi.Collections.Generic;

    public class Article : Aggregate<int>
    {
        private ArticleGuid _articleGuid = new ArticleGuid();
        private DateTime _createDate = DateTime.UtcNow;
        private string _description;
        private int _grossStock;
        private decimal _memberPrice;
        private decimal _publicPrice;
        private ISet<Image> _images = new HashedSet<Image>();
        private string _name;
        private Owner _owner;
        private string _specification;
        private StoreId _storeId;

        protected Article()
        {
        }

        [Obsolete]
        public Article(Owner owner, StoreId storeId, string name, int grossStock)
        {
            AssertionConcern.AssertArgumentNotNull(owner, "Owner must be provided.");
            AssertionConcern.AssertArgumentNotNull(storeId, "StoreId must be provided.");
            AssertionConcern.AssertArgumentNotEmpty(name, "Name must be provided.");
            AssertionConcern.AssertArgumentGreaterThan(grossStock, -1, "Gross stock must be greater or equal 0.");

            _owner = owner;
            _storeId = storeId;
            _name = name;
            _grossStock = grossStock;
        }

        public Article(Owner owner, StoreId storeId, ArticleGuid articleGuid, string name, int grossStock,
            decimal memberPrice, decimal publicPrice)
        {
            if (owner == null) throw new ArgumentNullException("owner");
            if (storeId == null) throw new ArgumentNullException("storeId");
            if (articleGuid == null) throw new ArgumentNullException("articleGuid");
            if (name == null) throw new ArgumentNullException("name");
            _owner = owner;
            _storeId = storeId;
            _articleGuid = articleGuid;
            _name = name;
            _grossStock = grossStock;
            _memberPrice = memberPrice;
            _publicPrice = publicPrice;
        }

        public virtual ArticleId ArticleId
        {
            get { return new ArticleId(Id); }
        }

        public virtual ArticleGuid ArticleGuid
        {
            get { return _articleGuid; }
        }

        public virtual Owner Owner
        {
            get { return _owner; }
            protected set { _owner = value; }
        }

        public virtual StoreId StoreId
        {
            get { return _storeId; }
        }

        public virtual ISet<Image> Images
        {
            get { return _images; }
            protected set { _images = value; }
        }

        public virtual DateTime CreateDate
        {
            get { return _createDate; }
            protected set { _createDate = value; }
        }

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public virtual string Brand { get; set; }

        [Obsolete]
        public virtual decimal Price { get; set; }

        public virtual decimal MemberPrice
        {
            get { return _memberPrice; }
            protected set { _memberPrice = value; }
        }

        public virtual decimal PublicPrice
        {
            get { return _publicPrice; }
            protected set { _publicPrice = value; }
        }

        public virtual int GrossStock
        {
            get { return _grossStock; }
            set { _grossStock = value; }
        }

        public virtual string Description
        {
            get { return _description; }
            protected set { _description = value; }
        }

        public virtual string Specification
        {
            get { return _specification; }
            protected set { _specification = value; }
        }

        public virtual string Color { get; set; }

        public virtual void ChangeDescription(string description)
        {
            if (_description != null && description == _description)
                return;

            Description = description;

            EventPublisher.Publish(new DescriptionChanged());
        }

        public virtual void ChangeSpecification(string specification)
        {
            if (_specification != null && specification == _specification)
                return;

            Specification = specification;

            EventPublisher.Publish(new SpecificationChanged());
        }

        public virtual Image AddImage(string fileName, string type, long length)
        {
            var image = new Image
            {
                FileName = fileName,
                Length = length,
                Type = type,
                Article = this,
                IsPreview = Images.Count == 0
            };
            Images.Add(image);

            EventPublisher.Publish(new ImageAdded());

            return image;
        }

        public virtual void RemoveImage(string fileName)
        {
            var image = Images.FirstOrDefault(p => p.FileName == fileName);
            if (image == null)
                return;
            Images.Remove(image);
            image.Article = null;

            EventPublisher.Publish(new ImageRemoved());

            EnsureOneImageIsPreviewImage();
        }

        private void EnsureOneImageIsPreviewImage()
        {
            if (Images.Count == 0)
                return;

            var previewImage = Images.FirstOrDefault(p => p.IsPreview);
            if (previewImage != null)
                return;

            previewImage = Images.FirstOrDefault();
            if (previewImage == null)
                return;

            previewImage.IsPreview = true;
            EventPublisher.Publish(new PreviewImageChanged());
        }

        public virtual void SetPreviewImage(string fileName)
        {
            foreach (var eachImage in Images)
            {
                eachImage.IsPreview = eachImage.FileName == fileName;
            }
        }
    }
}
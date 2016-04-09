namespace Phundus.Inventory.Articles.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Domain.Model;
    using Common.Eventing;
    using Iesi.Collections.Generic;
    using Inventory.Model;
    using Inventory.Model.Articles;

    public class Article : EntityWithCompositeId
    {
        private ArticleId _articleId = new ArticleId();
        private ArticleShortId _articleShortId;
        private DateTime _createDate = DateTime.UtcNow;
        private string _description;
        private int _grossStock;
        private Iesi.Collections.Generic.ISet<Image> _images = new HashedSet<Image>();
        private decimal? _memberPrice;
        private string _name;
        private Owner _owner;
        private decimal _publicPrice;
        private string _specification;
        private StoreId _storeId;
        private Iesi.Collections.Generic.ISet<Tag> _tags = new HashedSet<Tag>();

        protected Article()
        {
        }

        public Article(Owner owner, StoreId storeId, ArticleId articleId, ArticleShortId articleShortId, string name,
            int grossStock, decimal publicPrice, decimal? memberPrice)
        {
            if (owner == null) throw new ArgumentNullException("owner");
            if (storeId == null) throw new ArgumentNullException("storeId");
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (articleShortId == null) throw new ArgumentNullException("articleShortId");
            if (name == null) throw new ArgumentNullException("name");

            _owner = owner;
            _storeId = storeId;
            _articleId = articleId;
            _articleShortId = articleShortId;
            _name = name;
            _grossStock = grossStock;
            _memberPrice = _owner.Type == OwnerType.Organization ? memberPrice : null;
            _publicPrice = publicPrice;
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

        public virtual int Version { get; protected set; }

        public virtual Owner Owner
        {
            get { return _owner; }
            protected set { _owner = value; }
        }

        public virtual StoreId StoreId
        {
            get { return _storeId; }
        }

        public virtual Iesi.Collections.Generic.ISet<Image> Images
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


        public virtual decimal PublicPrice
        {
            get { return _publicPrice; }
            protected set { _publicPrice = value; }
        }

        public virtual decimal? MemberPrice
        {
            get { return _memberPrice; }
            protected set { _memberPrice = value; }
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

        public virtual OwnerId OwnerId
        {
            get { return Owner.OwnerId; }
        }

        public virtual Iesi.Collections.Generic.ISet<Tag> Tags
        {
            get { return _tags; }
            protected set { _tags = value; }
        }

        public virtual void ChangeDescription(Manager manager, string description)
        {
            if (_description != null && description == _description)
                return;

            Description = description;

            EventPublisher.Publish(new DescriptionChanged(manager, ArticleShortId, ArticleId, Owner.OwnerId,
                Description));
        }

        public virtual void ChangeSpecification(Manager manager, string specification)
        {
            if (_specification != null && specification == _specification)
                return;

            Specification = specification;

            EventPublisher.Publish(new SpecificationChanged(manager, ArticleShortId, ArticleId, Owner.OwnerId,
                Specification));
        }

        public virtual Image AddImage(Manager manager, string fileName, string type, long length)
        {
            if (Images.Count(p => p.FileName == fileName) > 0)
                throw new InvalidOperationException(String.Format("Image with file name {0} already exists.", fileName));

            var image = new Image
            {
                FileName = fileName,
                Length = length,
                Type = type,
                Article = this,
                IsPreview = Images.Count == 0
            };
            Images.Add(image);

            EventPublisher.Publish(new ImageAdded(manager, ArticleShortId, ArticleId, Owner.OwnerId, image.FileName,
                image.Type, image.Length, image.IsPreview));

            return image;
        }

        public virtual void RemoveImage(Manager manager, string fileName)
        {
            var image = Images.FirstOrDefault(p => p.FileName == fileName);
            if (image == null)
                return;
            Images.Remove(image);
            image.Article = null;

            EventPublisher.Publish(new ImageRemoved(manager, ArticleShortId, ArticleId, Owner.OwnerId, image.FileName));

            EnsureOneImageIsPreviewImage(manager);
        }

        private void EnsureOneImageIsPreviewImage(Manager manager)
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
            EventPublisher.Publish(new PreviewImageChanged(manager, ArticleShortId, ArticleId, Owner.OwnerId,
                previewImage.FileName, previewImage.Type, previewImage.Length));
        }

        public virtual void SetPreviewImage(Manager manager, string fileName)
        {
            var oldPreviewImage = Images.FirstOrDefault(p => p.IsPreview);
            foreach (var eachImage in Images)
            {
                eachImage.IsPreview = eachImage.FileName == fileName;
            }

            var previewImage = Images.FirstOrDefault(p => p.IsPreview);

            if ((oldPreviewImage == previewImage) || (previewImage == null))
                return;

            EventPublisher.Publish(new PreviewImageChanged(manager, ArticleShortId, ArticleId, Owner.OwnerId,
                previewImage.FileName, previewImage.Type, previewImage.Length));
        }

        public virtual void ChangePrices(Manager manager, decimal publicPrice, decimal? memberPrice)
        {
            if (manager == null) throw new ArgumentNullException("manager");

            if ((PublicPrice == publicPrice) && (MemberPrice == memberPrice))
                return;

            PublicPrice = publicPrice;
            MemberPrice = Owner.Type == OwnerType.Organization ? memberPrice : null;

            EventPublisher.Publish(new PricesChanged(manager, ArticleShortId, ArticleId, Owner.OwnerId, PublicPrice,
                MemberPrice));
        }

        public virtual void ChangeDetails(Manager manager, string name, string brand, string color)
        {
            if (manager == null) throw new ArgumentNullException("manager");

            if ((Name == name) && (Brand == brand) && (Color == color))
                return;

            Name = name;
            Brand = brand;
            Color = color;

            EventPublisher.Publish(new ArticleDetailsChanged(manager, ArticleShortId, ArticleId, Owner.OwnerId, Name,
                Brand, Color));
        }

        public virtual void ChangeGrossStock(Manager manager, int grossStock)
        {
            if (manager == null) throw new ArgumentNullException("manager");

            if (GrossStock == grossStock)
                return;

            var oldGrossStock = GrossStock;
            GrossStock = grossStock;

            EventPublisher.Publish(new GrossStockChanged(manager, ArticleShortId, ArticleId, Owner.OwnerId,
                oldGrossStock, GrossStock));
        }

        protected override IEnumerable<object> GetIdentityComponents()
        {
            yield return ArticleId;
        }

        public virtual void Tag(Manager manager, string name)
        {
            var tag = new Tag(name);
            if (_tags.Contains(tag))
                throw new InvalidOperationException(String.Format("Product is already tagged with {0}.", tag.Name));
            if (!_tags.Add(tag))
                throw new InvalidOperationException(String.Format("Could not add tag {0}.", name));

            EventPublisher.Publish(new ProductTagged(manager, ArticleId, OwnerId, tag.Name));
        }

        public virtual void Untag(Manager manager, string name)
        {
            var tag = new Tag(name);
            
            if (!_tags.Remove(tag))
                throw new InvalidOperationException(String.Format("Could not remove tag {0}.", name));

            EventPublisher.Publish(new ProductUntagged(manager, ArticleId, OwnerId, tag.Name));
        }
    }
}
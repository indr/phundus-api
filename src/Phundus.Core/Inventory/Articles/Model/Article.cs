namespace Phundus.Inventory.Articles.Model
{
    using System;
    using System.Linq;
    using Common.Domain.Model;
    using Ddd;
    using Iesi.Collections.Generic;

    public class Article : Aggregate<int>
    {
        private ArticleGuid _articleGuid = new ArticleGuid();
        private DateTime _createDate = DateTime.UtcNow;
        private string _description;
        private int _grossStock;
        private ISet<Image> _images = new HashedSet<Image>();
        private decimal? _memberPrice;
        private string _name;
        private Owner _owner;
        private decimal _publicPrice;
        private string _specification;
        private StoreId _storeId;

        protected Article()
        {
        }

        public Article(Owner owner, StoreId storeId, ArticleGuid articleGuid, string name, int grossStock,
            decimal publicPrice, decimal? memberPrice)
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
            _memberPrice = _owner.Type == OwnerType.Organization ? memberPrice : null;
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

        public virtual void ChangeDescription(Initiator initiator, string description)
        {
            if (_description != null && description == _description)
                return;

            Description = description;

            EventPublisher.Publish(new DescriptionChanged(initiator, ArticleId, ArticleGuid, Owner.OwnerId, Description));
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

        public virtual void ChangePrices(Initiator initiator, decimal publicPrice, decimal? memberPrice)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");

            if ((PublicPrice == publicPrice) && (MemberPrice == memberPrice))
                return;

            PublicPrice = publicPrice;
            MemberPrice = Owner.Type == OwnerType.Organization ? memberPrice : null;

            EventPublisher.Publish(new PricesChanged(initiator, Id, ArticleGuid, PublicPrice, MemberPrice));
        }
    }
}
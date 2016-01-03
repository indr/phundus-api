namespace Phundus.Core.Inventory.Articles.Model
{
    using System;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using Ddd;
    using Iesi.Collections.Generic;
    using Owners;

    public class Article : Aggregate<int>
    {
        private DateTime _createDate = DateTime.UtcNow;
        private string _description;
        private ISet<Image> _images = new HashedSet<Image>();
        private string _name;
        private Owner _owner;
        private string _specification;
        private StoreId _storeId;

        protected Article()
        {
        }

        public Article(Owner owner, StoreId storeId, string name)
        {
            AssertionConcern.AssertArgumentNotNull(owner, "Owner must be provided.");
            AssertionConcern.AssertArgumentNotNull(storeId, "StoreId must be provided.");
            AssertionConcern.AssertArgumentNotEmpty(name, "Name must be provided.");

            _owner = owner;
            _storeId = storeId;
            _name = name;
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

        public virtual decimal Price { get; set; }

        public virtual int GrossStock { get; set; }

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
                Article = this
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
        }
    }
}
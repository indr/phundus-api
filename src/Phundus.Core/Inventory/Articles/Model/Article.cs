﻿namespace Phundus.Core.Inventory.Articles.Model
{
    using System;
    using System.Linq;
    using Common;
    using Iesi.Collections.Generic;
    using Owners;

    public class Article
    {
        private string _caption;
        private DateTime _createDate = DateTime.Now;
        private ISet<Image> _images = new HashedSet<Image>();
        private Owner _owner;

        protected Article()
        {
        }

        public Article(Owner owner, string name)
        {
            AssertionConcern.AssertArgumentNotNull(owner, "Owner must be provided.");

            _owner = owner;
            _caption = name;
        }

        public virtual int Id { get; protected set; }

        public virtual int Version { get; protected set; }

        public virtual Owner Owner
        {
            get { return _owner; }
            protected set { _owner = value; }
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
    }
}
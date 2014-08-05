namespace Phundus.Core.Inventory.Model
{
    using System;
    using Iesi.Collections.Generic;
    using Microsoft.Practices.ServiceLocation;
    using Shop.Orders.Repositories;

    public class Article
    {
        private string _caption;
        private DateTime _createDate = DateTime.Now;
        private ISet<Image> _images = new HashedSet<Image>();
        private int _organizationId;

        protected Article()
        {
        }

        public Article(int organizationId, string name)
        {
            _organizationId = organizationId;
            _caption = name;
        }

        private IOrderRepository OrderRepository
        {
            get { return ServiceLocator.Current.GetInstance<IOrderRepository>(); }
        }

        public virtual int Id { get; protected set; }

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

        /// <summary>
        /// Reservierbarer Bestand
        /// </summary>
        public virtual int ReservableStock
        {
            get { return GrossStock - OrderRepository.SumReservedAmount(Id); }
        }

        public virtual string Color { get; set; }

        public virtual bool AddImage(Image image)
        {
            var result = Images.Add(image);
            image.Article = this;
            return result;
        }

        public virtual bool RemoveImage(Image image)
        {
            var result = Images.Remove(image);
            image.Article = null;
            return result;
        }
    }
}
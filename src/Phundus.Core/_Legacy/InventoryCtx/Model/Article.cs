namespace Phundus.Core.InventoryCtx.Model
{
    using System;
    using System.Linq;
    using Ddd;
    using Exceptions;
    using Iesi.Collections.Generic;
    using Microsoft.Practices.ServiceLocation;
    using Repositories;
    using ReservationCtx.Repositories;
    using Shop.Orders.Repositories;

    public class Article : EntityBase
    {
        private DateTime _createDate;
        private ISet<Image> _images = new HashedSet<Image>();

        public Article()
        {
            _createDate = DateTime.Now;
        }

        public Article(int id, int version)
            : base(id, version)
        {
            _createDate = DateTime.Now;
        }

        protected virtual IOrderRepository OrderRepository
        {
            get { return ServiceLocator.Current.GetInstance<IOrderRepository>(); }
        }

        //public virtual Organization Organization { get; set; }
        public virtual int OrganizationId { get; set; }

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

        public virtual string Name { get; set; }

        public virtual string Brand { get; set; }

        public virtual double Price { get; set; }

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
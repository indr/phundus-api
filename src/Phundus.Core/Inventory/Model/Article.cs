namespace Phundus.Core.Inventory.Model
{
    using System;
    using Ddd;
    using Iesi.Collections.Generic;
    using Microsoft.Practices.ServiceLocation;
    using Shop.Orders.Repositories;

    public class Article 
    {
        private IOrderRepository OrderRepository
        {
            get { return ServiceLocator.Current.GetInstance<IOrderRepository>(); }
        }

        private DateTime _createDate = DateTime.Now;
        private ISet<Image> _images = new HashedSet<Image>();
        private int _organizationId;
        private string _caption;

        private Article()
        {
            
        }

        public Article(int organizationId, string name)
        {
            _organizationId = organizationId;
            _caption = name;
        }

        public int Id { get; protected set; }

        public int Version { get; protected set; }

        public int OrganizationId
        {
            get { return _organizationId; }
            protected set { _organizationId = value; }
        }

        public ISet<Image> Images
        {
            get { return _images; }
            set { _images = value; }
        }

        public DateTime CreateDate
        {
            get { return _createDate; }
            protected set { _createDate = value; }
        }

        public string Caption
        {
            get { return _caption; }
            set { _caption = value; }
        }

        public string Brand { get; set; }

        public decimal Price { get; set; }

        public int GrossStock { get; set; }

        public string Description { get; set; }

        public string Specification { get; set; }

        /// <summary>
        /// Reservierbarer Bestand
        /// </summary>
        public int ReservableStock
        {
            get { return GrossStock - OrderRepository.SumReservedAmount(Id); }
        }

        public string Color { get; set; }

        public bool AddImage(Image image)
        {
            var result = Images.Add(image);
            image.Article = this;
            return result;
        }

        public bool RemoveImage(Image image)
        {
            var result = Images.Remove(image);
            image.Article = null;
            return result;
        }
    }
}
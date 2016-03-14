namespace Phundus.Inventory.Projections
{
    using System;
    using System.Text;
    using Common.Eventing;
    using Common.Projecting;
    using Model.Stores;
    using Stores.Model;

    public class StoreDetailsProjection : ProjectionBase<StoreDetailsData>,
        ISubscribeTo<ContactDetailsChanged>,
        ISubscribeTo<CoordinateChanged>,
        ISubscribeTo<OpeningHoursChanged>,
        ISubscribeTo<StoreOpened>,
        ISubscribeTo<StoreRenamed>
    {
        public void Handle(ContactDetailsChanged e)
        {
            Update(e.StoreId, x =>
            {
                x.Line1 = e.Line1;
                x.Line2 = e.Line2;
                x.Street = e.Street;
                x.Postcode = e.Postcode;
                x.City = e.City;
                x.PostalAddress = MakePostalAddress(e);
                x.EmailAddress = e.EmailAddress;
                x.PhoneNumber = e.PhoneNumber;
            });
        }

        public void Handle(CoordinateChanged e)
        {
            Update(e.StoreId, x =>
            {
                x.Latitude = e.Latitude;
                x.Longitude = e.Longitude;
            });
        }

        public void Handle(OpeningHoursChanged e)
        {
            Update(e.StoreId, x =>
                x.OpeningHours = e.OpeningHours);
        }

        public void Handle(StoreOpened e)
        {
            Insert(x =>
            {
                x.StoreId = e.StoreId;
                x.OwnerId = e.Owner.OwnerId.Id;
                x.OwnerType = e.Owner.Type.ToString().ToLowerInvariant();
            });
        }

        public void Handle(StoreRenamed e)
        {
            Update(e.StoreId, x =>
                x.Name = e.Name);
        }

        private static string MakePostalAddress(ContactDetailsChanged e)
        {
            var sb = new StringBuilder();
            if (!String.IsNullOrWhiteSpace(e.Line1))
                sb.AppendLine(e.Line1);
            if (!String.IsNullOrWhiteSpace(e.Line2))
                sb.AppendLine(e.Line2);
            if (!String.IsNullOrWhiteSpace(e.Street))
                sb.AppendLine(e.Street);
            if (!String.IsNullOrWhiteSpace(e.Postcode) && !String.IsNullOrWhiteSpace(e.City))
                sb.AppendLine(e.Postcode + " " + e.City);
            else if (!String.IsNullOrWhiteSpace(e.Postcode))
                sb.AppendLine(e.Postcode);
            else if (!String.IsNullOrWhiteSpace(e.City))
                sb.AppendLine(e.City);
            return sb.ToString();
        }
    }

    public class StoreDetailsData
    {
        public virtual Guid OwnerId { get; set; }
        public virtual string OwnerType { get; set; }
        public virtual Guid StoreId { get; set; }
        public virtual string Name { get; set; }

        public virtual string Line1 { get; set; }
        public virtual string Line2 { get; set; }
        public virtual string Street { get; set; }
        public virtual string Postcode { get; set; }
        public virtual string City { get; set; }
        public virtual string PostalAddress { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual string PhoneNumber { get; set; }

        public virtual string OpeningHours { get; set; }
        public virtual decimal? Latitude { get; set; }
        public virtual decimal? Longitude { get; set; }
    }
}
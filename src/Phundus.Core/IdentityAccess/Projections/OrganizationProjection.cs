namespace Phundus.IdentityAccess.Projections
{
    using System;
    using System.Text;
    using Common;
    using Common.Notifications;
    using Common.Projecting;
    using Model;
    using Model.Organizations;
    using Organizations.Model;

    public class OrganizationProjection : ProjectionBase<OrganizationData>,
        IConsumes<OrganizationEstablished>,
        IConsumes<OrganizationContactDetailsChanged>,
        IConsumes<OrganizationPlanChanged>,
        IConsumes<PublicRentalSettingChanged>,
        IConsumes<StartpageChanged>
    {
        public void Consume(OrganizationContactDetailsChanged e)
        {
            Update(e.OrganizationId, x =>
            {
                x.Line1 = e.Line1;
                x.Line2 = e.Line2;
                x.Street = e.Street;
                x.Postcode = e.Postcode;
                x.City = e.City;
                x.PostalAddress = MakePostalAddress(e);
                x.PhoneNumber = e.PhoneNumber;
                x.EmailAddress = e.EmailAddress;
                x.Website = e.Website;
            });
        }

        public void Consume(OrganizationEstablished e)
        {
            Insert(x =>
            {
                x.OrganizationId = e.OrganizationId;
                x.Name = e.Name;
                x.Url = e.Name.ToFriendlyUrl();
                x.Plan = e.Plan;
                x.EstablishedAtUtc = e.OccuredOnUtc;
                x.PublicRental = e.PublicRental;
            });
        }

        public void Consume(OrganizationPlanChanged e)
        {
            Update(e.OrganizationId, x =>
                x.Plan = e.NewPlan);
        }

        public void Consume(PublicRentalSettingChanged e)
        {
            Update(e.OrganizationId, x =>
                x.PublicRental = e.Value);
        }

        public void Consume(StartpageChanged e)
        {
            Update(e.OrganizationId, x =>
                x.Startpage = e.Startpage);
        }

        private static string MakePostalAddress(OrganizationContactDetailsChanged e)
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

    public class OrganizationData
    {
        private string _website;

        public virtual Guid OrganizationId { get; set; }
        public virtual DateTime EstablishedAtUtc { get; set; }
        public virtual string Name { get; set; }
        public virtual string Url { get; set; }
        public virtual string Plan { get; set; }
        public virtual bool PublicRental { get; set; }

        public virtual string Line1 { get; set; }
        public virtual string Line2 { get; set; }
        public virtual string Street { get; set; }
        public virtual string Postcode { get; set; }
        public virtual string City { get; set; }
        public virtual string PostalAddress { get; set; }

        public virtual string EmailAddress { get; set; }
        public virtual string PhoneNumber { get; set; }

        public virtual string Website
        {
            get
            {
                if (!String.IsNullOrEmpty(_website) && !_website.StartsWith("http"))
                    return "http://" + _website;
                return _website;
            }
            set { _website = value; }
        }

        public virtual string Startpage { get; set; }
        public virtual string DocumentTemplate { get; set; }
    }
}
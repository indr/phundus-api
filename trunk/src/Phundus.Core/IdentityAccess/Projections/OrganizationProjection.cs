namespace Phundus.IdentityAccess.Projections
{
    using System;
    using System.Text;
    using Common;
    using Common.Domain.Model;
    using Common.Notifications;
    using Cqrs;    
    using Model;
    using Model.Organizations;
    using Organizations.Model;

    public class OrganizationProjection : ProjectionBase<OrganizationData>, IStoredEventsConsumer
    {
        public void Handle(DomainEvent e)
        {
            Process((dynamic) e);
        }

        private void Process(DomainEvent e)
        {
            // Noop
        }

        private void Process(OrganizationEstablished e)
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

        private void Process(OrganizationPlanChanged e)
        {
            Update(e.OrganizationId, x =>
                x.Plan = e.NewPlan);
        }

        private void Process(OrganizationContactDetailsChanged e)
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

        private string MakePostalAddress(OrganizationContactDetailsChanged e)
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

        private void Process(PublicRentalSettingChanged e)
        {
            Update(e.OrganizationId, x =>
                x.PublicRental = e.Value);
        }

        private void Process(StartpageChanged e)
        {
            Update(e.OrganizationId, x =>
                x.Startpage = e.Startpage);
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
namespace Phundus.IdentityAccess.Projections
{
    using System;
    using System.Text;
    using Application;
    using Common;
    using Common.Eventing;
    using Common.Projecting;
    using Model;
    using Model.Organizations;
    using Organizations.Model;

    public class OrganizationProjection : ProjectionBase<OrganizationData>,
        ISubscribeTo<OrganizationEstablished>,
        ISubscribeTo<OrganizationContactDetailsChanged>,
        ISubscribeTo<OrganizationPlanChanged>,
        ISubscribeTo<PdfTemplateChanged>,
        ISubscribeTo<PublicRentalSettingChanged>,
        ISubscribeTo<StartpageChanged>
    {
        public void Handle(OrganizationContactDetailsChanged e)
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

        public void Handle(OrganizationEstablished e)
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

        public void Handle(OrganizationPlanChanged e)
        {
            Update(e.OrganizationId, x =>
                x.Plan = e.NewPlan);
        }

        public void Handle(PdfTemplateChanged e)
        {
            Update(e.OrganizationId, x =>
                x.PdfTemplateFileName = e.PdfTemplateFileName);
        }

        public void Handle(PublicRentalSettingChanged e)
        {
            Update(e.OrganizationId, x =>
                x.PublicRental = e.Value);
        }

        public void Handle(StartpageChanged e)
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
}
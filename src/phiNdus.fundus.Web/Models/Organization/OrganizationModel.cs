namespace phiNdus.fundus.Web.Models.Organization
{
    using System;
    using Phundus.Core.OrganizationAndMembershipCtx.Queries;

    public class OrganizationModel
    {
        public OrganizationModel(OrganizationDetailDto organization)
        {
            Id = organization.Id;
            Name = organization.Name;
            Address = organization.Address;
            Coordinate = organization.Coordinate;
            Startpage = organization.Startpage;
            EmailAddress = organization.EmailAddress;
            Website = organization.Website;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Coordinate { get; set; }

        public string Startpage { get; set; }

        public bool HasOptionJoin { get; set; }

        public bool HasOptionLeave { get; set; }

        public bool HasOptions
        {
            get { return HasOptionJoin || HasOptionLeave; }
        }

        public string EmailAddress { get; set; }

        public string Website { get; set; }

        public bool HasContactOptions
        {
            get
            {
                return !String.IsNullOrWhiteSpace(Address) || !String.IsNullOrWhiteSpace(EmailAddress) ||
                       !String.IsNullOrWhiteSpace(Website);
            }
        }
    }
}
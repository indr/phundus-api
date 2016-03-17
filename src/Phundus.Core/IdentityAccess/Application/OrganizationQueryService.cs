namespace Phundus.IdentityAccess.Application
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Common.Querying;

    public interface IOrganizationQueryService
    {
        OrganizationData GetById(Guid organizationId);
        OrganizationData FindById(Guid organizationId);
        IEnumerable<OrganizationData> Query();
    }

    public class OrganizationQueryService : QueryServiceBase<OrganizationData>, IOrganizationQueryService
    {
        public OrganizationData GetById(Guid organizationId)
        {
            var result = FindById(organizationId);
            if (result == null)
                throw new NotFoundException("Organization {0} not found.", organizationId);
            return result;
        }

        public OrganizationData FindById(Guid organizationId)
        {
            return SingleOrDefault(p => p.OrganizationId == organizationId);
        }

        public IEnumerable<OrganizationData> Query()
        {
            return QueryOver().OrderBy(p => p.Name).Asc.List();
        }
    }

    // TODO: Split to OrganizationListData and OrganizationDetailsData
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
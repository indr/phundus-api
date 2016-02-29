namespace Phundus.IdentityAccess.Projections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Cqrs;
    using Integration.IdentityAccess;
    using Organizations.Model;
    using Organizations.Repositories;

    public interface IOrganizationQueries
    {
        IEnumerable<IOrganization> All();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        IOrganization GetById(Guid organizationId);

        IOrganization FindById(Guid organizationId);
    }

    public class OrganizationsProjection : ProjectionBase, IOrganizationQueries
    {
        public IOrganizationRepository OrganizationRepository { get; set; }

        public IOrganization GetById(Guid organizationId)
        {
            var result = FindById(organizationId);
            if (result == null)
                throw new NotFoundException(String.Format("Organization {0} not found.", organizationId));
            return result;
        }

        public IOrganization FindById(Guid id)
        {
            var organization = OrganizationRepository.FindById(id);
            if (organization == null)
                return null;

            return ToOrganizationDetailDto(organization);
        }

        public IEnumerable<IOrganization> All()
        {
            return OrganizationRepository.FindAll()
                .Select(ToOrganizationDetailDto).ToList();
        }

        public IEnumerable<IOrganization> ByMemberId(Guid memberId)
        {
            Organization orgAlias = null;
            Membership memberAlias = null;
            var query = Session.QueryOver(() => orgAlias)
                .JoinAlias(() => orgAlias.Memberships, () => memberAlias)
                .Where(() => memberAlias.UserId.Id == memberId);

            var result = new List<IOrganization>();
            foreach (var each in query.List())
            {
                result.Add(ToOrganizationDetailDto(each));
            }
            return result;
        }

        private static OrganizationData ToOrganizationDetailDto(Organization organization)
        {
            var result = new OrganizationData
            {
                OrganizationId = organization.Id.Id,
                EstablishedAtUtc = organization.EstablishedAtUtc,
                Name = organization.Name,
                Url = organization.FriendlyUrl,
                Startpage = organization.Startpage,
                DocumentTemplate = organization.DocTemplateFileName,
                PublicRental = organization.Settings.PublicRental,
                Plan = organization.Plan.ToString()
            };

            if (organization.ContactDetails != null)
            {
                var cd = organization.ContactDetails;
                result.PostalAddress = cd.GetPostalAddress();
                result.Line1 = cd.Line1;
                result.Line2 = cd.Line2;
                result.Street = cd.Street;
                result.Postcode = cd.Postcode;
                result.City = cd.City;
                result.EmailAddress = cd.EmailAddress;
                result.Website = cd.Website;
                result.PhoneNumber = cd.PhoneNumber;
            }

            return result;
        }
    }

    public class OrganizationData : IOrganization
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
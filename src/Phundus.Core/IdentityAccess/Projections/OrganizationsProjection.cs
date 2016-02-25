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

        private static OrganizationViewRow ToOrganizationDetailDto(Organization organization)
        {
            var result = new OrganizationViewRow
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
                result.PostAddress = organization.ContactDetails.PostAddress;
                result.EmailAddress = organization.ContactDetails.EmailAddress;
                result.Website = organization.ContactDetails.Website;
                result.PhoneNumber = organization.ContactDetails.PhoneNumber;
            }

            return result;
        }
    }

    public class OrganizationViewRow : IOrganization
    {
        private string _website;

        public Guid OrganizationId { get; set; }
        public DateTime EstablishedAtUtc { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public string PostAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }

        public string Website
        {
            get
            {
                if (!String.IsNullOrEmpty(_website) && !_website.StartsWith("http"))
                    return "http://" + _website;
                return _website;
            }
            set { _website = value; }
        }

        public string Startpage { get; set; }
        public string DocumentTemplate { get; set; }
        public bool PublicRental { get; set; }
        public string Plan { get; set; }
    }
}
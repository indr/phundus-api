namespace Phundus.IdentityAccess.Queries.ReadModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Cqrs;
    using Organizations.Model;
    using Organizations.Repositories;

    public class OrganizationsReadModel : ReadModelBase, IOrganizationQueries
    {
        public IOrganizationRepository OrganizationRepository { get; set; }

        public IEnumerable<OrganizationDto> ByMemberId(Guid memberId)
        {
            Organization orgAlias = null;
            Membership memberAlias = null;
            var query = Session.QueryOver(() => orgAlias)
                .JoinAlias(() => orgAlias.Memberships, () => memberAlias)
                .Where(() => memberAlias.UserGuid.Id == memberId);

            var result = new List<OrganizationDto>();
            foreach (var each in query.List())
            {
                result.Add(ToOrganizationDto(each));
            }
            return result;
        }

        public OrganizationDetailDto GetById(Guid organizationId)
        {
            var result = FindById(organizationId);
            if (result == null)
                throw new NotFoundException(String.Format("Organization {0} not found.", organizationId));
            return result;
        }

        public OrganizationDetailDto FindById(Guid id)
        {
            var organization = OrganizationRepository.FindById(id);
            if (organization == null)
                return null;

            return ToOrganizationDetailDto(organization);
        }

        public IEnumerable<OrganizationDto> All()
        {
            return OrganizationRepository.FindAll()
                .Select(ToOrganizationDto).ToList();
        }

        private static OrganizationDetailDto ToOrganizationDetailDto(Organization organization)
        {
            var result = new OrganizationDetailDto
            {                
                Guid = organization.Id,
                OrganizationId = organization.Id,
                Version = organization.Version,
                EstablishedAtUc = organization.EstablishedAtUtc,
                Name = organization.Name,
                Url = organization.Url,
                
                Startpage = organization.Startpage,
                CreateDate = organization.EstablishedAtUtc,
                DocumentTemplate = organization.DocTemplateFileName
            };

            if (organization.ContactDetails != null)
            {
                result.Address = organization.ContactDetails.PostAddress;
                result.EmailAddress = organization.ContactDetails.EmailAddress;
                result.Website = organization.ContactDetails.Website;
                result.PhoneNumber = organization.ContactDetails.PhoneNumber;
            }

            return result;
        }

        private static OrganizationDto ToOrganizationDto(Organization organization)
        {
            var result = new OrganizationDto
            {                
                Guid = organization.Id,
                Version = organization.Version,
                Name = organization.Name,
                Url = organization.Url,
                EstablishedAtUtc = organization.EstablishedAtUtc,
            };

            if (organization.ContactDetails != null)
                result.Address = organization.ContactDetails.PostAddress;

            return result;
        }
    }

    public class OrganizationDto
    {
        [Obsolete]
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public int Version { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Address { get; set; }
        public DateTime EstablishedAtUtc { get; set; }
    }

    public class OrganizationDetailDto
    {
        [Obsolete]
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public Guid OrganizationId { get; set; }
        public int Version { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Address { get; set; }

        public string EmailAddress { get; set; }
        public string Website { get; set; }
        public string Coordinate { get; set; }
        public string Startpage { get; set; }
        public DateTime CreateDate { get; set; }
        public string DocumentTemplate { get; set; }
        public DateTime EstablishedAtUc { get; set; }
        public string PhoneNumber { get; set; }
    }
}
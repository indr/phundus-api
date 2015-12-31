namespace Phundus.Core.IdentityAndAccess.Queries
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

        public IEnumerable<OrganizationDto> ByMemberId(int memberId)
        {
            Organization orgAlias = null;
            Membership memberAlias = null;
            var query = Session.QueryOver(() => orgAlias)
                .JoinAlias(() => orgAlias.Memberships, () => memberAlias)
                .Where(() => memberAlias.UserId == memberId);

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
                throw new NotFoundException(String.Format("Organization with id {0} not found.", organizationId));
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
            return new OrganizationDetailDto
            {                
                Guid = organization.Id,
                OrganizationId = organization.Id,
                Version = organization.Version,
                Name = organization.Name,
                Url = organization.Url,
                Address = organization.Address,
                EmailAddress = organization.EmailAddress,
                Website = organization.Website,
                Startpage = organization.Startpage,
                CreateDate = organization.CreateDate,
                DocumentTemplate = organization.DocTemplateFileName
            };
        }

        private static OrganizationDto ToOrganizationDto(Organization organization)
        {
            return new OrganizationDto
            {                
                Guid = organization.Id,
                Version = organization.Version,
                Name = organization.Name,
                Url = organization.Url,
                Address = organization.Address
            };
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
        
    }
}
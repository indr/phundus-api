namespace phiNdus.fundus.Web.Controllers.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Castle.Transactions;
    using Domain.Entities;
    using Domain.Repositories;
    using Dtos;
    using NHibernate;

    public class OrganizationsController : ApiControllerBase
    {
        public IUserRepository Users { get; set; }
        public IOrganizationRepository Organizations { get; set; }

        public Func<ISession> SessionFactory { get; set; }

        [Transaction]
        public virtual IEnumerable<OrganizationListDto> Get()
        {
            return Organizations
                .FindAll()
                .Select(each => new OrganizationListDto
                    {
                        Id = each.Id,
                        Version = each.Version,
                        Name = each.Name,
                        Url = each.Url,
                        Address = each.Address
                    })
                .ToList();
        }

        // GET api/organizations/5
        [Transaction]
        public virtual OrganizationDto Get(int id)
        {
            var result = Organizations.FindById(id);
            if (result == null)
                throw new HttpNotFoundException("Die Organisation ist nicht vorhanden.");

            return ToDto(result);
        }

        private static OrganizationDto ToDto(Organization organization)
        {
            return new OrganizationDto
                {
                    Id = organization.Id,
                    Version = organization.Version,
                    Name = organization.Name,
                    Url = organization.Url,
                    Address = organization.Address,
                    EmailAddress = organization.EmailAddress,
                    Website = organization.Website,
                    Coordinate = organization.Coordinate,
                    Startpage = organization.Startpage,
                    CreateDate = organization.CreateDate,
                    DocumentTemplate = organization.DocTemplateFileName
                };
        }

        // PUT api/organizations/5
        [Transaction]
        [Authorize]
        public virtual OrganizationDto Put(int id, [FromBody] OrganizationDto value)
        {
            var org = Organizations.FindById(id);

            var user = Users.FindByEmail(User.Identity.Name);

            if (org == null)
                throw new HttpNotFoundException("Die Organisation ist nicht vorhanden.");

            if (user == null || !user.IsChiefOf(org))
                throw new HttpForbiddenException("Sie haben keine Berechtigung um die Organisation zu aktualisieren.");

            if (org.Version != value.Version)
                throw new HttpConflictException("Die Organisation wurde in der Zwischenzeit verändert.");

            org.Address = value.Address;
            org.EmailAddress = value.EmailAddress;
            org.Website = value.Website;
            org.Coordinate = value.Coordinate;
            org.Startpage = value.Startpage;
            org.DocTemplateFileName = value.DocumentTemplate;

            Organizations.Update(org);
            SessionFactory().Flush();

            return ToDto(org);
        }
    }
}
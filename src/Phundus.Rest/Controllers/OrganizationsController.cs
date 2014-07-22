namespace Phundus.Rest.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Castle.Transactions;
    using Core.IdentityAndAccessCtx.Repositories;
    using Core.OrganizationAndMembershipCtx.Model;
    using Core.OrganizationAndMembershipCtx.Queries;
    using Core.OrganizationAndMembershipCtx.Repositories;
    using Dtos;
    using Exceptions;
    using NHibernate;

    public class OrganizationsController : ApiControllerBase
    {
        public IUserRepository Users { get; set; }
        public IOrganizationRepository Organizations { get; set; }

        // TODO: Remove dependency on NHibernate
        public Func<ISession> SessionFactory { get; set; }

        [Transaction]
        public virtual IEnumerable<OrganizationDto> Get()
        {
            return Organizations
                .FindAll()
                .Select(each => new OrganizationDto
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
        public virtual OrganizationDetailDto Get(int id)
        {
            var result = Organizations.FindById(id);
            if (result == null)
                throw new HttpNotFoundException("Die Organisation ist nicht vorhanden.");

            return ToDto(result);
        }

        private static OrganizationDetailDto ToDto(Organization organization)
        {
            return new OrganizationDetailDto
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
        public virtual OrganizationDetailDto Put(int id, [FromBody] OrganizationDetailDto value)
        {
            throw new NotSupportedException();
            //var org = Organizations.FindById(id);

            //var user = Users.FindByEmail(Identity.Name);

            //if (org == null)
            //    throw new HttpNotFoundException("Die Organisation ist nicht vorhanden.");

            //if (user == null || !user.IsChiefOf(org))
            //    throw new HttpForbiddenException("Sie haben keine Berechtigung um die Organisation zu aktualisieren.");

            //if (org.Version != value.Version)
            //    throw new HttpConflictException("Die Organisation wurde in der Zwischenzeit verändert.");

            //org.Address = value.Address;
            //org.EmailAddress = value.EmailAddress;
            //org.Website = value.Website;
            //org.Coordinate = value.Coordinate;
            //org.Startpage = value.Startpage;
            //org.DocTemplateFileName = value.DocumentTemplate;

            //Organizations.Update(org);
            //SessionFactory().Flush();

            //return ToDto(org);
        }
    }
}
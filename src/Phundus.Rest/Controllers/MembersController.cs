namespace Phundus.Rest.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using Castle.Transactions;
    using Core.IdentityAndAccessCtx.Repositories;
    using Core.OrganisationCtx;
    using Dtos;
    using Exceptions;

    [Authorize(Roles="Chief")]
    public class MembersController : ApiControllerBase
    {
        public IUserRepository Users { get; set; }
        public IOrganizationRepository Organizations { get; set; }
        public IUserRepository Members { get; set; }

        [Transaction]
        public virtual MemberDtos Get(int organization)
        {
            var org = Organizations.FindById(organization);
            var user = Users.FindByEmail(Identity.Name);

            if (org == null)
                throw new HttpNotFoundException("Die Organisation ist nicht vorhanden.");

            if (!user.IsChiefOf(org))
                throw new HttpForbiddenException("Sie haben keine Berechtigung um die Mitglieder zu lesen.");

            var result = new MemberDtos();
            var members = Members.FindByOrganization(organization);

            foreach (var each in members)
            {
                var membership = each.Memberships.First(p => p.Organization.Id == organization);
                //result.Add(ToDto(membership));
                result.Add(Map<MemberDto>(membership));
            }

            return result;
        }

        [HttpPut]
        [Transaction]
        public virtual void Approve(int organization, int id)
        {
            var membership = DoSomeStuffIDontHaveWordsFor(organization, id);
            membership.Approve();
            //Members.Update(member);
        }

        [HttpPut]
        [Transaction]
        public virtual void Lock(int organization, int id)
        {
            var membership = DoSomeStuffIDontHaveWordsFor(organization, id);
            membership.Lock();
            //Members.Update(member);
        }

        [HttpPut]
        [Transaction]
        public virtual void Unlock(int organization, int id)
        {
            var membership = DoSomeStuffIDontHaveWordsFor(organization, id);
            membership.Unlock();
            //Members.Update(member);
        }

        [HttpPut]
        [Transaction]
        public virtual void SetRole(int organization, int id, [FromBody] int roleId)
        {
            var membership = DoSomeStuffIDontHaveWordsFor(organization, id);
            membership.Role = roleId;
            //Members.Update(member);
        }

        private OrganizationMembership DoSomeStuffIDontHaveWordsFor(int organization, int id)
        {
            var org = Organizations.FindById(organization);
            var user = Users.FindByEmail(Identity.Name);

            if (org == null)
                throw new HttpNotFoundException("Die Organisation ist nicht vorhanden.");

            if (user == null || !user.IsChiefOf(org))
                throw new HttpForbiddenException("Sie haben keine Berechtigung um die Mitglieder zu lesen.");

            var member = Members.FindById(id);
            if (member == null)
                throw new HttpNotFoundException("Das Mitglied ist nicht vorhanden.");

            var membership = member.Memberships.FirstOrDefault(p => p.Organization.Id == organization);
            if (membership == null)
                throw new HttpNotFoundException("Die Mitgliedschaft ist nicht vorhanden.");

            return membership;
        }
    }
}
namespace Phundus.Rest.Controllers.IdentityAndAccess
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Organizations.Commands;
    using Core.IdentityAndAccess.Organizations.Repositories;
    using Core.IdentityAndAccess.Queries;

    public class MembersController : ApiControllerBase
    {
        public IOrganizationRepository Organizations { get; set; }

        public IMembershipQueries MembershipQueries { get; set; }

        public IMemberQueries MemberQueries { get; set; }

        [Transaction]
        public virtual IList<MemberDto> Get(int organizationId)
        {
            return MemberQueries.ByOrganizationId(organizationId);
        }

        [Transaction]
        public virtual void Post(int organizationId, dynamic doc)
        {
            Dispatcher.Dispatch(new AllowMembershipApplication
            {
                InitiatorId = CurrentUserId,
                ApplicationId = doc.applicationId
            });
        }

        [HttpPut]
        [Transaction]
        public virtual void Approve(int organizationId, int id)
        {
            throw new NotSupportedException();
            //var membership = DoSomeStuffIDontHaveWordsFor(organization, id);
            //membership.Approve();
            ////Members.Update(member);
        }

        [HttpPut]
        [Transaction]
        public virtual void Lock(int organizationId, int id)
        {
            throw new NotSupportedException();
            //var membership = DoSomeStuffIDontHaveWordsFor(organization, id);
            //membership.Lock();
            ////Members.Update(member);
        }

        [HttpPut]
        [Transaction]
        public virtual void Unlock(int organizationId, int id)
        {
            throw new NotSupportedException();
            //var membership = DoSomeStuffIDontHaveWordsFor(organization, id);
            //membership.Unlock();
            ////Members.Update(member);
        }

        [HttpPut]
        [Transaction]
        public virtual void SetRole(int organizationId, int id, dynamic doc)
        {
            Dispatcher.Dispatch(new ChangeMembersRole
            {
                OrganizationId = organizationId,
                InitiatorId = CurrentUserId,
                MemberId = id,
                Role = doc.role
            });
        }

        //private OrganizationMembership DoSomeStuffIDontHaveWordsFor(int organizationId, int id)
        //{
        //    var org = Organizations.FindById(organization);
        //    var user = Users.FindByEmail(Identity.Name);

        //    if (org == null)
        //        throw new HttpNotFoundException("Die Organisation ist nicht vorhanden.");

        //    if (user == null || !user.IsChiefOf(org))
        //        throw new HttpForbiddenException("Sie haben keine Berechtigung um die Mitglieder zu lesen.");

        //    var member = Members.FindById(id);
        //    if (member == null)
        //        throw new HttpNotFoundException("Das Mitglied ist nicht vorhanden.");

        //    var membership = member.Memberships.FirstOrDefault(p => p.Organization.Id == organization);
        //    if (membership == null)
        //        throw new HttpNotFoundException("Die Mitgliedschaft ist nicht vorhanden.");

        //    return membership;
        //}
    }
}
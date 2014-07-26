﻿namespace Phundus.Rest.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Organizations.Repositories;
    using Core.IdentityAndAccess.Queries;
    using Core.IdentityAndAccess.Users.Repositories;

    [Authorize]
    public class MembersController : ApiControllerBase
    {
        public IUserRepository Users { get; set; }
        public IOrganizationRepository Organizations { get; set; }
        public IUserRepository Members { get; set; }

        public IUserQueries UserQueries { get; set; }

        public IMembershipQueries MembershipQueries { get; set; }

        public IMemberQueries MemberQueries { get; set; }

        [Transaction]
        public virtual IList<MemberDto> Get(int organization)
        {
            return MemberQueries.ByOrganizationId(organization);
        }

        [HttpPut]
        [Transaction]
        public virtual void Approve(int organization, int id)
        {
            throw new NotSupportedException();
            //var membership = DoSomeStuffIDontHaveWordsFor(organization, id);
            //membership.Approve();
            ////Members.Update(member);
        }

        [HttpPut]
        [Transaction]
        public virtual void Lock(int organization, int id)
        {
            throw new NotSupportedException();
            //var membership = DoSomeStuffIDontHaveWordsFor(organization, id);
            //membership.Lock();
            ////Members.Update(member);
        }

        [HttpPut]
        [Transaction]
        public virtual void Unlock(int organization, int id)
        {
            throw new NotSupportedException();
            //var membership = DoSomeStuffIDontHaveWordsFor(organization, id);
            //membership.Unlock();
            ////Members.Update(member);
        }

        [HttpPut]
        [Transaction]
        public virtual void SetRole(int organization, int id, [FromBody] int roleId)
        {
            throw new NotSupportedException();
            //var membership = DoSomeStuffIDontHaveWordsFor(organization, id);
            //membership.Role = roleId;
            //Members.Update(member);
        }

        //private OrganizationMembership DoSomeStuffIDontHaveWordsFor(int organization, int id)
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
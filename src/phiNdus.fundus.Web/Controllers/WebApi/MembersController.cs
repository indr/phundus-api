namespace phiNdus.fundus.Web.Controllers.WebApi
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using Castle.Transactions;
    using Domain.Entities;
    using Domain.Repositories;
    using Dtos;
    using NHibernate;

    public class MembersController : ApiController
    {
        public IUserRepository Users { get; set; }
        public IOrganizationRepository Organizations { get; set; }
        public IMemberRepository Members { get; set; }

        public Func<ISession> SessionFactory { get; set; }

        [Transaction]
        public virtual MemberDtos Get(int organization)
        {
            var org = Organizations.FindById(organization);
            var user = Users.FindByEmail(User.Identity.Name);

            if (org == null)
                throw new HttpNotFoundException("Die Organisation ist nicht vorhanden.");

            if (!user.IsChiefOf(org))
                throw new HttpForbiddenException("Sie haben keine Berechtigung um die Mitglieder zu lesen.");

            var result = new MemberDtos();
            var members = Members.FindByOrganization(organization);

            foreach (var each in members)
            {
                var membership = each.Memberships.First(p => p.Organization.Id == organization);
                result.Add(ToDto(each, membership));
            }

            return result;
        }

        [HttpPut]
        [Transaction]
        public virtual void Approve(int organization, int id)
        {
            var org = Organizations.FindById(organization);
            var user = Users.FindByEmail(User.Identity.Name);

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

            //if ((member.Version != value.MemberVersion)
            //    || (membership.Version != value.MembershipVersion))
            //    throw new HttpConflictException(
            //        "Das Mitglied oder die Mitgliedschaft wurde in der Zwischenzeit verändert.");

            membership.Approve();

            //Members.Update(member);
        }

        [HttpPut]
        [Transaction]
        public virtual void Lock(int organization, int id)
        {
            var org = Organizations.FindById(organization);
            var user = Users.FindByEmail(User.Identity.Name);

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

            //if ((member.Version != value.MemberVersion)
            //    || (membership.Version != value.MembershipVersion))
            //    throw new HttpConflictException(
            //        "Das Mitglied oder die Mitgliedschaft wurde in der Zwischenzeit verändert.");

            membership.Lock();

            //Members.Update(member);
        }

        [HttpPut]
        [Transaction]
        public virtual void Unlock(int organization, int id)
        {
            var org = Organizations.FindById(organization);
            var user = Users.FindByEmail(User.Identity.Name);

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

            //if ((member.Version != value.MemberVersion)
            //    || (membership.Version != value.MembershipVersion))
            //    throw new HttpConflictException(
            //        "Das Mitglied oder die Mitgliedschaft wurde in der Zwischenzeit verändert.");

            membership.Unlock();

            //Members.Update(member);
        }

        [HttpPut]
        [Transaction]
        public virtual void SetRole(int organization, int id, [FromBody] int roleId)
        {
            var org = Organizations.FindById(organization);
            var user = Users.FindByEmail(User.Identity.Name);

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

            //if ((member.Version != value.MemberVersion)
            //    || (membership.Version != value.MembershipVersion))
            //    throw new HttpConflictException(
            //        "Das Mitglied oder die Mitgliedschaft wurde in der Zwischenzeit verändert.");

            membership.Role = roleId;

            //Members.Update(member);
        }

        private static MemberDto ToDto(User member, OrganizationMembership membership)
        {
            return new MemberDto
                {
                    Id = member.Id,
                    MemberVersion = member.Version,
                    MembershipVersion = membership.Version,
                    FirstName = member.FirstName,
                    LastName = member.LastName,
                    JsNumber = member.JsNumber,
                    EmailAddress = member.Membership.Email,
                    Role = membership.Role,
                    RequestDate = membership.RequestDate,
                    IsLockedOut = membership.IsLockedOut,
                    LastLockoutDate = membership.LastLockoutDate,
                    IsApproved = membership.IsApproved,
                    ApprovalDate = membership.ApprovalDate
                };
        }
    }
}
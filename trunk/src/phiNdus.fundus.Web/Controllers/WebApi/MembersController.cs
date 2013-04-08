namespace phiNdus.fundus.Web.Controllers.WebApi
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Castle.Transactions;
    using phiNdus.fundus.Domain.Entities;
    using phiNdus.fundus.Domain.Repositories;

    public class MembersController : ApiController
    {
        public IMemberRepository Members { get; set; }

        // GET api/{organization}/members
        [Transaction]
        public virtual IEnumerable<MemberListDto> Get(int organization)
        {
            var result = new List<MemberListDto>();
            var members = Members.FindByOrganization(organization);

            foreach (var each in members)
            {
                var membership = each.Memberships.First(p => p.Organization.Id == organization);
                result.Add(ToDto(each, membership));
            }

            return result;
        }

        // GET api/{organization}/members/5
        //public MemberDto Get(int organization, int id)
        //{
        //    throw new NotImplementedException();
        //}

        // POST api/{organization}/members
        //public void Post(int organization, [FromBody] MemberDto value)
        //{
        //    throw new NotImplementedException();
        //}

        // PUT api/{organization}/members/5
        [Transaction]
        public virtual MemberDto Put(int organization, int id, [FromBody] MemberDto value)
        {
            var member = Members.FindById(value.Id);

            if (member.Membership.IsLockedOut != value.IsLocked)
            {
                if (value.IsLocked)
                    member.Membership.LockOut();
                else if (value.IsLocked == false)
                    member.Membership.Unlock();
            }

            var membership = member.Memberships.First(p => p.Organization.Id == organization);
            membership.Role = value.Role;

            return ToDto(member, membership);
        }

        // DELETE api/{organization}/members/5
        //public void Delete(int organization, int id)
        //{
        //    throw new NotImplementedException();
        //}

        static MemberDto ToDto(User member, OrganizationMembership membership)
        {
            return new MemberDto
                       {
                           EmailAddress = member.Membership.Email,
                           FirstName = member.FirstName,
                           Id = member.Id,
                           IsLocked = member.Membership.IsLockedOut,
                           JsNumber = member.JsNumber,
                           LastName = member.LastName,
                           Role = membership.Role
                       };
        }
    }

    public class MemberListDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? JsNumber { get; set; }
        public string EmailAddress { get; set; }
        public int Role { get; set; }
        public bool IsLocked { get; set; }
    }

    public class MemberDto : MemberListDto
    {
    }
}
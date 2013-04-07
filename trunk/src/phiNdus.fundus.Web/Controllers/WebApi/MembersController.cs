namespace phiNdus.fundus.Web.Controllers.WebApi
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Castle.Transactions;
    using Domain.Repositories;

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
                result.Add(new MemberListDto()
                    {
                        EmailAddress = each.Membership.Email,
                        FirstName = each.FirstName,
                        Id = each.Id,
                        IsLocked = each.Membership.IsLockedOut,
                        LastName = each.LastName,
                        Role = membership.Role
                    });
            }

            return result;
        }

        // GET api/{organization}/members/5
        //public MemberDto Get(string organization, int id)
        //{
        //    return new MemberDto
        //        {
        //            EmailAddress = "admin@test.phundus.ch",
        //            FirstName = "Kari",
        //            LastName = "Zuppiger",
        //            IsLocked = false
        //        };
        //}

        // POST api/{organization}/members
        //public void Post(string organization, [FromBody] MemberDto value)
        //{
        //}

        // PUT api/{organization}/members/5
        [Transaction]
        public virtual MemberDto Put(string organization, int id, [FromBody] MemberDto value)
        {
            value.Role = 0;
            return value;
        }

        // DELETE api/{organization}/members/5
        //public void Delete(string organization, int id)
        //{
        //}
    }

    public class MemberListDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public int Role { get; set; }
        public bool IsLocked { get; set; }
    }

    public class MemberDto : MemberListDto
    {
    }
}
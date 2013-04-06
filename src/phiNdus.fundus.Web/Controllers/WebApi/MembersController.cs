using System.Collections.Generic;
using System.Web.Http;

namespace phiNdus.fundus.Web.Controllers.WebApi
{
    public class MembersController : ApiController
    {
        // GET api/{organization}/members
        public IEnumerable<MemberListDto> Get(string organization)
        {
            return new List<MemberListDto>
                {
                    new MemberListDto
                        {
                            EmailAddress = "user@test.phundus.ch",
                            FirstName = "Hans",
                            LastName = "Müller",
                            IsLocked = false
                        },
                    new MemberListDto
                        {
                            EmailAddress = "admin@test.phundus.ch",
                            FirstName = "Kari",
                            LastName = "Zuppiger",
                            IsLocked = false
                        },
                    new MemberListDto
                        {
                            EmailAddress = "peter@test.phundus.ch",
                            FirstName = "peter",
                            LastName = "Müller",
                            IsLocked = true
                        }
                };
        }

        // GET api/{organization}/members/5
        public MemberDto Get(string organization, int id)
        {
            return new MemberDto
                {
                    EmailAddress = "admin@test.phundus.ch",
                    FirstName = "Kari",
                    LastName = "Zuppiger",
                    IsLocked = false
                };
        }

        // POST api/{organization}/members
        public void Post(string organization, [FromBody] MemberDto value)
        {
        }

        // PUT api/{organization}/members/5
        public void Put(string organization, int id, [FromBody] MemberDto value)
        {
        }

        // DELETE api/{organization}/members/5
        public void Delete(string organization, int id)
        {
        }
    }

    public class MemberListDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public bool IsLocked { get; set; }
    }

    public class MemberDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public bool IsLocked { get; set; }
    }
}
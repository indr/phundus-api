namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System;
    using System.Collections.Generic;

    public interface IMemberQueries
    {
        IList<MemberDto> ByOrganizationId(int organizationId);
    }

    public class MembersReadModel : IMemberQueries
    {
        public IUserQueries UserQueries { get; set; }

        public IMembershipQueries MembershipQueries { get; set; }

        public IList<MemberDto> ByOrganizationId(int organizationId)
        {
            var result = new List<MemberDto>();
            var memberships = MembershipQueries.ByOrganizationId(organizationId);
            foreach (var each in memberships)
            {
                var user = UserQueries.ById(each.MemberId);

                result.Add(new MemberDto
                {
                    Id = user.Id,
                    EmailAddress = user.Email,
                    FirstName = user.FirstName,
                    JsNumber = user.JsNumber,
                    LastName = user.LastName,
                    IsApproved = true,
                    IsLockedOut = false,
                    Role = each.MembershipRole == "Chief" ? 2 : 1
                });
            }

            return result;
        }
    }

    public class MemberDto
    {
        public int Id { get; set; }
        public int MemberVersion { get; set; }
        public int MembershipVersion { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? JsNumber { get; set; }
        public string EmailAddress { get; set; }

        public int Role { get; set; }

        public DateTime? RequestDate { get; set; }

        public bool IsLockedOut { get; set; }
        public DateTime? LastLockoutDate { get; set; }

        public bool IsApproved { get; set; }
        public DateTime? ApprovalDate { get; set; }
    }

    public class MemberDtos : List<MemberDto>
    {
    }
}
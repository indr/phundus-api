namespace Phundus.Core.OrganizationAndMembershipCtx.Queries
{
    using System;
    using System.Collections.Generic;
    using IdentityAndAccessCtx.Queries;

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
                    Role = each.MembershipRole
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

        public string Role { get; set; }

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
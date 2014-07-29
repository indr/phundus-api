namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IMemberQueries
    {
        IList<MemberDto> ByOrganizationId(int organizationId);
    }

    public interface IMemberInRoleQueries
    {
        IList<MemberDto> Chiefs(int organizationId);
    }

    public class MembersReadModel : IMemberQueries, IMemberInRoleQueries
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
                    ApprovalDate = each.ApprovedOn,
                    Role = each.MembershipRole == "Chief" ? 2 : 1,
                    IsLocked = each.IsLocked
                });
            }

            return result;
        }

        public IList<MemberDto> Chiefs(int organizationId)
        {
            return this.ByOrganizationId(organizationId).Where(p => p.Role == 2).ToList();
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

        public bool IsLocked { get; set; }

        public DateTime? ApprovalDate { get; set; }
    }

    public class MemberDtos : List<MemberDto>
    {
    }
}
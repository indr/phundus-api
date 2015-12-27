namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Organizations.Model;
    using Organizations.Repositories;

    public class MembershipsReadModel : IMembershipQueries
    {
        public IUserQueries UserQueries { get; set; }

        public IMembershipRepository MembershipRepository { get; set; }

        public IList<MembershipDto> ByUserId(int userId)
        {
            return MembershipRepository.ByMemberId(userId).Select(ToMembershipDto).ToList();
        }

        public IList<MembershipDto> ByUserName(string userName)
        {
            var user = UserQueries.ByUserName(userName);
            if (user == null)
                return new List<MembershipDto>();

            return ByUserId(user.Id);
        }

        public IList<MembershipDto> FindByOrganizationId(Guid organizationId)
        {
            return MembershipRepository.GetByOrganizationId(organizationId).Select(ToMembershipDto).ToList();
        }

        private static MembershipDto ToMembershipDto(Membership each)
        {
            return new MembershipDto
            {
                Id = each.Id,
                MemberId = each.UserId,                
                OrganizationGuid = each.Organization.Id,
                OrganizationName = each.Organization.Name,
                OrganizationUrl = each.Organization.Url,
                ApprovedOn = each.ApprovalDate,
                MembershipRole = each.Role.ToString(),
                IsLocked = each.IsLocked
            };
        }
    }

    public class MembershipDto
    {
        public Guid Id { get; set; }
        public int MemberId { get; set; }
        [Obsolete]
        public int OrganizationId { get; set; }
        public Guid OrganizationGuid { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationUrl { get; set; }
        public string MembershipRole { get; set; }
        public DateTime ApprovedOn { get; set; }
        public bool IsLocked { get; set; }
    }
}
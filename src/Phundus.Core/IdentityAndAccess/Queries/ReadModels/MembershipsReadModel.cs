namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using IdentityAndAccess.Organizations.Model;
    using IdentityAndAccess.Organizations.Repositories;

    public class MembershipsReadModel : IMembershipQueries
    {
        public IMembershipRepository MembershipRepository { get; set; }

        public IList<MembershipDto> ByMemberId(int memberId)
        {
            return MembershipRepository.ByMemberId(memberId).Select(ToMembershipDto).ToList();
        }

        public IList<MembershipDto> ByOrganizationId(int organizationId)
        {
            return MembershipRepository.ByOrganizationId(organizationId).Select(ToMembershipDto).ToList();
        }

        private static MembershipDto ToMembershipDto(Membership each)
        {
            return new MembershipDto
            {
                Id = each.Id,
                MemberId = each.UserId,
                OrganizationId = each.Organization.Id,
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
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationUrl { get; set; }
        public string MembershipRole { get; set; }
        public DateTime ApprovedOn { get; set; }
        public bool IsLocked { get; set; }
    }
}
namespace Phundus.IdentityAccess.Application
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Querying;
    using Model.Organizations;
    using Organizations.Model;

    public interface IMembershipQueryService
    {
        MembershipData Find(Guid organizationId, Guid userId);
        ICollection<MembershipData> FindByOrganizationId(Guid organizationId);
        ICollection<MembershipData> FindByUserId(Guid userId);
    }

    public class MembershipsProjection : QueryServiceBase, IMembershipQueryService
    {
        private readonly IMembershipRepository _membershipRepository;

        public MembershipsProjection(IMembershipRepository membershipRepository)
        {
            if (membershipRepository == null) throw new ArgumentNullException("membershipRepository");

            _membershipRepository = membershipRepository;
        }

        public MembershipData Find(Guid organizationId, Guid userId)
        {
            var result = _membershipRepository.Find(organizationId, userId);
            if (result == null)
                return null;
            return ToMembershipData(result);
        }

        public ICollection<MembershipData> FindByOrganizationId(Guid organizationId)
        {
            return _membershipRepository.FindByOrganizationId(organizationId).Select(ToMembershipData).ToList();
        }

        public ICollection<MembershipData> FindByUserId(Guid userId)
        {
            return _membershipRepository.FindByUserId(userId).Select(ToMembershipData).ToList();
        }

        private static MembershipData ToMembershipData(Membership each)
        {
            return new MembershipData
            {
                Id = each.Id,
                UserGuid = each.UserId.Id,
                OrganizationGuid = each.Organization.Id.Id,
                OrganizationName = each.Organization.Name,
                OrganizationUrl = each.Organization.FriendlyUrl,
                ApprovedOn = each.ApprovedAtUtc,
                MembershipRole = each.MemberRole.ToString(),
                IsLocked = each.IsLocked,
                RecievesEmailNotifications = each.RecievesEmailNotifications
            };
        }
    }

    public class MembershipData
    {
        public Guid Id { get; set; }
        public int MemberId { get; set; }
        public Guid UserGuid { get; set; }
        public Guid OrganizationGuid { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationUrl { get; set; }
        public string MembershipRole { get; set; }
        public DateTime ApprovedOn { get; set; }
        public bool IsLocked { get; set; }
        public bool RecievesEmailNotifications { get; set; }
    }
}
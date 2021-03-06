﻿namespace Phundus.IdentityAccess.Application
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Domain.Model;
    using Common.Querying;

    public interface IMemberQueryService
    {
        IList<MemberData> FindByOrganizationId(Guid organizationId);
        IEnumerable<MemberData> Query(CurrentUserId currentUserId, Guid queryOrganizationId, string queryFullName);
        ICollection<MemberData> Managers(Guid organizationId, bool emailSubscribtion);
    }

    public class MembersProjection : QueryServiceBase, IMemberQueryService
    {
        private readonly IMembershipQueryService _membershipQueryService;
        private readonly IUserQueryService _userQueries;

        public MembersProjection(IMembershipQueryService membershipQueryService, IUserQueryService userQueries)
        {
            if (membershipQueryService == null) throw new ArgumentNullException("membershipQueryService");
            if (userQueries == null) throw new ArgumentNullException("userQueries");
            _membershipQueryService = membershipQueryService;
            _userQueries = userQueries;
        }

        public IList<MemberData> FindByOrganizationId(Guid organizationId)
        {
            var memberships = _membershipQueryService.FindByOrganizationId(organizationId);
            return ToMemberData(memberships);
        }

        public IEnumerable<MemberData> Query(CurrentUserId currentUserId, Guid queryOrganizationId, string queryFullName)
        {
            var memberships = _membershipQueryService.FindByOrganizationId(queryOrganizationId);
            return ToMemberData(memberships, queryFullName);
        }

        public ICollection<MemberData> Managers(Guid organizationId, bool emailSubscribtion)
        {
            return FindByOrganizationId(organizationId)
                .Where(p => p.Role == 2)
                .Where(p => p.RecievesEmailNotifications == emailSubscribtion).ToList();
        }

        private IList<MemberData> ToMemberData(IEnumerable<MembershipData> memberships, string queryFullName = "")
        {
            queryFullName = queryFullName == null ? null : queryFullName.ToLowerInvariant();
            var result = new List<MemberData>();
            foreach (var each in memberships)
            {
                var user = _userQueries.GetById(each.UserGuid);
                if (!String.IsNullOrWhiteSpace(queryFullName) &&
                    (!user.FirstName.ToLowerInvariant().Contains(queryFullName) &&
                     !user.LastName.ToLowerInvariant().Contains(queryFullName)))
                    continue;

                result.Add(new MemberData
                {
                    Guid = user.UserId,
                    EmailAddress = user.EmailAddress,
                    FirstName = user.FirstName,
                    JsNumber = user.JsNummer,
                    LastName = user.LastName,
                    ApprovalDate = each.ApprovedOn,
                    Role = each.MembershipRole == "Manager" ? 2 : 1,
                    RecievesEmailNotifications = each.RecievesEmailNotifications,
                    IsLocked = each.IsLocked
                });
            }

            return result;
        }
    }

    public class MemberData
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
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

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        public bool RecievesEmailNotifications { get; set; }
    }

    public class MemberDtos : List<MemberData>
    {
    }
}
namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Domain.Model;
    using Integration.IdentityAccess;

    public interface IMemberQueries
    {
        IList<MemberDto> FindByOrganizationId(Guid organizationId);
        IEnumerable<MemberDto> Query(CurrentUserId currentUserId, Guid queryOrganizationId, string queryFullName);
    }

    public class MembersReadModel : IMemberQueries, IMembersWithRole
    {
        public IUserQueries UserQueries { get; set; }

        public IMembershipQueries MembershipQueries { get; set; }

        public IList<MemberDto> FindByOrganizationId(Guid organizationId)
        {
            var memberships = MembershipQueries.FindByOrganizationId(organizationId);
            return ToMemberDtos(memberships);
        }

        public IEnumerable<MemberDto> Query(CurrentUserId currentUserId, Guid queryOrganizationId, string queryFullName)
        {
            // TODO: Members Read-Model 
            var memberships = MembershipQueries.FindByOrganizationId(queryOrganizationId);
            return ToMemberDtos(memberships, queryFullName);
        }

        public IList<Manager> Manager(Guid tenantId)
        {
            return
                FindByOrganizationId(tenantId)
                    .Where(p => p.Role == 2)
                    .Select(s => new Manager(s.Guid, s.FullName, s.EmailAddress))
                    .ToList();
        }

        private IList<MemberDto> ToMemberDtos(IEnumerable<MembershipDto> memberships, string queryFullName = "")
        {
            queryFullName = queryFullName == null ? null : queryFullName.ToLowerInvariant();
            var result = new List<MemberDto>();
            foreach (var each in memberships)
            {
                var user = UserQueries.GetById(each.MemberId);
                if (!String.IsNullOrWhiteSpace(queryFullName) &&
                    (!user.FirstName.ToLowerInvariant().Contains(queryFullName) &&
                     !user.LastName.ToLowerInvariant().Contains(queryFullName)))
                    continue;

                result.Add(new MemberDto
                {
                    Id = user.UserId,
                    Guid = user.UserGuid,
                    EmailAddress = user.EmailAddress,
                    FirstName = user.FirstName,
                    JsNumber = user.JsNummer,
                    LastName = user.LastName,
                    ApprovalDate = each.ApprovedOn,
                    Role = each.MembershipRole == "Chief" ? 2 : 1,
                    IsLocked = each.IsLocked
                });
            }

            return result;
        }
    }

    public class MemberDto
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
    }

    public class MemberDtos : List<MemberDto>
    {
    }
}
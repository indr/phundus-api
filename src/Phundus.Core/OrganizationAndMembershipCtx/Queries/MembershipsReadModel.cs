namespace Phundus.Core.OrganizationAndMembershipCtx.Queries
{
    using System;
    using System.Collections.Generic;
    using Repositories;

    public interface IMembershipQueries
    {
        IList<MembershipDto> ByMemberId(int memberId);
    }

    public class MembershipsReadModel : IMembershipQueries
    {
        public IMembershipRepository MembershipRepository { get; set; }

        public IList<MembershipDto> ByMemberId(int memberId)
        {
            var result = new List<MembershipDto>();
            foreach (var each in MembershipRepository.ByMemberId(memberId))
            {
                result.Add(new MembershipDto
                {
                    Id = each.Id,
                    MemberId = each.MemberId,
                    OrganizationId = each.OrganizationId,
                    OrganizationName = each.Organization.Name
                });
            }

            return result;
        }
    }

    public class MembershipDto
    {
        public Guid Id { get; set; }
        public int MemberId { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
    }
}
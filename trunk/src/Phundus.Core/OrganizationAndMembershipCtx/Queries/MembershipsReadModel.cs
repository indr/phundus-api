﻿namespace Phundus.Core.OrganizationAndMembershipCtx.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Model;
    using Repositories;

    public interface IMembershipQueries
    {
        IList<MembershipDto> ByMemberId(int memberId);
        IList<MembershipDto> ByOrganizationId(int organizationId);
    }

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
                MemberId = each.MemberId,
                OrganizationId = each.OrganizationId,
                OrganizationName = each.Organization.Name,
                OrganizationUrl = each.Organization.Url,
                MembershipRole = (each.Role == 2 ? "Chief" : "Member")
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
    }
}
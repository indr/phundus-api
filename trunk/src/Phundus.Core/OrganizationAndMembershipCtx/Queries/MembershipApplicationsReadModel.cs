namespace Phundus.Core.OrganizationAndMembershipCtx.Queries
{
    using System;
    using System.Collections.Generic;
    using Castle.Transactions;
    using Ddd;
    using Model;
    using Repositories;

    public interface IMembershipApplicationQueries
    {
        MembershipApplicationDtos PendingByOrganizationId(int organizationId);
    }

    public class MembershipApplicationsReadModel : IMembershipApplicationQueries,
        ISubscribeTo<MembershipRequested>, ISubscribeTo<MembershipRequestApproved>,
        ISubscribeTo<MembershipRequestRejected>
    {
        public IMembershipRequestRepository MembershipRequestRepository { get; set; }

        [Transaction]
        public MembershipApplicationDtos PendingByOrganizationId(int organizationId)
        {
            var requests = MembershipRequestRepository.PendingByOrganization(organizationId);

            var result = new MembershipApplicationDtos();

            foreach (var each in requests)
            {
                result.Add(ToMembershipApplicationDto(each));
            }

            return result;
        }

        public void Handle(MembershipRequestApproved @event)
        {
            throw new NotImplementedException();
        }

        public void Handle(MembershipRequested @event)
        {
            throw new NotImplementedException();
        }

        public void Handle(MembershipRequestRejected @event)
        {
            throw new NotImplementedException();
        }

        private static MembershipApplicationDto ToMembershipApplicationDto(MembershipRequest each)
        {
            return new MembershipApplicationDto
            {
                Id = each.Id,
                OrgId = each.OrganizationId,
                UserId = each.MemberId,
                CreatedOn = each.RequestDate,
                ApprovedOn = each.ApprovalDate,
                RejectedOn = each.RejectDate
            };
        }
    }

    public class MembershipApplicationDto
    {
        public Guid Id { get; set; }

        public int OrgId { get; set; }

        public int UserId { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public DateTime? RejectedOn { get; set; }
    }

    public class MembershipApplicationDtos : List<MembershipApplicationDto>
    {
    }
}
namespace Phundus.Core.OrganizationAndMembershipCtx.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Castle.Transactions;
    using Ddd;
    using IdentityAndAccessCtx.Queries;
    using Model;
    using Repositories;

    public interface IMembershipApplicationQueries
    {
        IList<MembershipApplicationDto> PendingByOrganizationId(int organizationId);
    }

    public class MembershipApplicationsReadModel : IMembershipApplicationQueries,
        ISubscribeTo<MembershipRequested>, ISubscribeTo<MembershipRequestApproved>,
        ISubscribeTo<MembershipRequestRejected>
    {
        public IMembershipRequestRepository MembershipRequestRepository { get; set; }

        public IUserQueries UserQueries { get; set; }

        [Transaction]
        public IList<MembershipApplicationDto> PendingByOrganizationId(int organizationId)
        {
            var requests = MembershipRequestRepository.PendingByOrganization(organizationId);

            return requests.Select(x => ToMembershipApplicationDto(x, UserQueries.ById(x.MemberId))).ToList();
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

        private static MembershipApplicationDto ToMembershipApplicationDto(MembershipRequest membershipRequest, UserDto user)
        {
            return new MembershipApplicationDto
            {
                Id = membershipRequest.Id,
                OrgId = membershipRequest.OrganizationId,
                UserId = membershipRequest.MemberId,
                CreatedOn = membershipRequest.RequestDate,
                ApprovedOn = membershipRequest.ApprovalDate,
                RejectedOn = membershipRequest.RejectDate,

                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                JsNumber = user.JsNumber
            };
        }
    }

    public class MembershipApplicationDto
    {
        public Guid Id { get; set; }

        public int OrgId { get; set; }

        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int? JsNumber { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public DateTime? RejectedOn { get; set; }
    }
}
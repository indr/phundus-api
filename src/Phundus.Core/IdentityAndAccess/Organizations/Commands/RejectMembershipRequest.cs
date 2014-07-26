namespace Phundus.Core.IdentityAndAccess.Organizations.Commands
{
    using System;
    using Castle.Transactions;
    using Cqrs;
    using Infrastructure;
    using Repositories;

    public class RejectMembershipRequest
    {
        public Guid RequestId { get; set; }
        public int MemberId { get; set; }
    }

    public class RejectMembershipRequestHandler : IHandleCommand<RejectMembershipRequest>
    {
        public IMembershipRequestRepository MembershipRequestRepository { get; set; }

        public IOrganizationRepository OrganizationRepository { get; set; }

        [Transaction]
        public void Handle(RejectMembershipRequest command)
        {
            var request = MembershipRequestRepository.ById(command.RequestId);
            Guard.Against<EntityNotFoundException>(request == null, "Membership request not found");

            var organization = OrganizationRepository.ById(request.OrganizationId);
            Guard.Against<EntityNotFoundException>(organization == null, "Organization not found");
            organization.RejectMembershipRequest(request);
        }
    }
}
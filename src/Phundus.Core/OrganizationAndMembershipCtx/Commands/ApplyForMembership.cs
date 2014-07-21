namespace Phundus.Core.OrganizationAndMembershipCtx.Commands
{
    #region

    using System;
    using Castle.Transactions;
    using Cqrs;
    using Infrastructure;
    using Model;
    using Queries;
    using Repositories;
    using Services;

    #endregion

    public class ApplyForMembership
    {
        public int MemberId { get; set; }
        public int OrganizationId { get; set; }
    }

    public class ApproveMembershipRequestHandler : IHandleCommand<ApproveMembershipRequest>
    {
        public IMembershipRequestRepository Requests { get; set; }

        public IMembershipRepository Memberships { get; set; }

        public IOrganizationRepository OrganizationRepository { get; set; }

        [Transaction]
        public void Handle(ApproveMembershipRequest command)
        {
            var request = Requests.ById(command.RequestId);
            Guard.Against<EntityNotFoundException>(request == null, "Membership request not found");

            var organization = OrganizationRepository.ById(request.OrganizationId);
            Guard.Against<EntityNotFoundException>(organization == null, "Organization not found");
            var membership = organization.ApproveMembershipRequest(request, Memberships.NextIdentity());

            Memberships.Add(membership);
        }
    }

    public class RejectMembershipRequestHandler : IHandleCommand<RejectMembershipRequest>
    {
        public IMembershipRequestRepository Requests { get; set; }

        public IOrganizationRepository OrganizationRepository { get; set; }

        [Transaction]
        public void Handle(RejectMembershipRequest command)
        {
            var request = Requests.ById(command.RequestId);
            Guard.Against<EntityNotFoundException>(request == null, "Membership request not found");

            var organization = OrganizationRepository.ById(request.OrganizationId);
            Guard.Against<EntityNotFoundException>(organization == null, "Organization not found");
            organization.RejectMembershipRequest(request);
        }
    }

    public class ApplyForMembershipHandler : IHandleCommand<ApplyForMembership>
    {
        public IMembershipApplicationsReadModel ReadModel { get; set; }

        public IOrganizationRepository OrganizationRepository { get; set; }

        public IMemberService Members { get; set; }

        public IMembershipRequestRepository Requests { get; set; }

        [Transaction]
        public void Handle(ApplyForMembership command)
        {
            Organization organization = OrganizationRepository.ById(command.OrganizationId);
            Guard.Against<EntityNotFoundException>(organization == null, "Organization not found");

            var member = Members.MemberFrom(command.MemberId);
            Guard.Against<EntityNotFoundException>(member == null, "Member not found");

            var request = organization.RequestMembership(
                Requests.NextIdentity(),
                member
                );

            Requests.Add(request);
        }
    }

    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message) : base(message)
        {
        }
    }
}
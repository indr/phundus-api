namespace Phundus.Core.OrganisationCtx.Commands
{
    #region

    using System;
    using Castle.Transactions;
    using Cqrs;
    using DomainModel;
    using Phundus.Infrastructure;
    using ReadModel;
    using Repositories;
    using Services;

    #endregion

    public class ApplyForMembership
    {
        public int MemberId { get; set; }
        public int OrganizationId { get; set; }
    }

    public class ApproveMembershipRequest
    {
        public Guid RequestId { get; set; }
    }

    public class RejectMembershipRequest
    {
        public Guid RequestId { get; set; }
    }

    public class ApproveMembershipRequestHandler : IHandleCommand<ApproveMembershipRequest>
    {
        public IMembershipRequestRepository Requests { get; set; }

        public IOrganisationRepository Organisations { get; set; }

        [Transaction]
        public void Handle(ApproveMembershipRequest command)
        {
            var request = Requests.ById(command.RequestId);
            Guard.Against<EntityNotFoundException>(request == null, "Membership request not found");

            var organisation = Organisations.ById(request.OrganizationId);
            Guard.Against<EntityNotFoundException>(organisation == null, "Organization not found");
            organisation.ApproveMembershipRequest(request);
        }
    }

    public class RejectMembershipRequestHandler : IHandleCommand<RejectMembershipRequest>
    {
        public IMembershipRequestRepository Requests { get; set; }

        public IOrganisationRepository Organisations { get; set; }

        [Transaction]
        public void Handle(RejectMembershipRequest command)
        {
            var request = Requests.ById(command.RequestId);
            Guard.Against<EntityNotFoundException>(request == null, "Membership request not found");

            var organisation = Organisations.ById(request.OrganizationId);
            Guard.Against<EntityNotFoundException>(organisation == null, "Organization not found");
            organisation.RejectMembershipRequest(request);
        }
    }

    public class ApplyForMembershipHandler : IHandleCommand<ApplyForMembership>
    {
        public IMembershipApplicationsReadModel ReadModel { get; set; }

        public IOrganisationRepository Organisations { get; set; }

        public IMemberService Members { get; set; }

        public IMembershipRequestRepository Requests { get; set; }

        [Transaction]
        public void Handle(ApplyForMembership command)
        {
            Organisation organization = Organisations.ById(command.OrganizationId);
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
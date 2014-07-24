namespace Phundus.Core.OrganizationAndMembershipCtx.Commands
{
    using System;
    using Castle.Transactions;
    using Cqrs;
    using Infrastructure;
    using Model;
    using Queries;
    using Repositories;
    using Services;

    public class ApplyForMembership
    {
        public int UserId { get; set; }
        public int OrganizationId { get; set; }
    }

    public class ApplyForMembershipHandler : IHandleCommand<ApplyForMembership>
    {
        public IMembershipApplicationQueries ReadModel { get; set; }

        public IOrganizationRepository OrganizationRepository { get; set; }

        public IMemberService Members { get; set; }

        public IMembershipRequestRepository Requests { get; set; }

        [Transaction]
        public void Handle(ApplyForMembership command)
        {
            Organization organization = OrganizationRepository.ById(command.OrganizationId);
            Guard.Against<EntityNotFoundException>(organization == null, "Organization not found");

            var member = Members.MemberFrom(command.UserId);
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
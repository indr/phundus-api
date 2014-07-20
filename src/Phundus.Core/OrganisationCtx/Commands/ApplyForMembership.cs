namespace Phundus.Core.OrganisationCtx.Commands
{
    using System;
    using Castle.Transactions;
    using Cqrs;
    using DomainModel;
    using Phundus.Infrastructure;
    using ReadModel;
    using Repositories;
    using Services;

    public class ApplyForMembership
    {
        public int MemberId { get; set; }
        public int OrganizationId { get; set; }
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
            var organization = Organisations.ById(command.OrganizationId);
            Guard.Against<EntityNotFoundException>(organization == null, "Organization not found");

            var member = Members.MemberFrom(command.MemberId);
            Guard.Against<EntityNotFoundException>(member == null, "Member not found");


            throw new NotImplementedException();
            //var request = organization.RequestMembershipFor(member);
            //Requests.Add(request);
        }
    }

    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message) : base(message)
        {
            
        }
    }
}
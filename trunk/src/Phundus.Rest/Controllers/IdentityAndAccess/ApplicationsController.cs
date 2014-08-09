namespace Phundus.Rest.Controllers.IdentityAndAccess
{
    using System;
    using System.Collections.Generic;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Organizations.Commands;
    using Core.IdentityAndAccess.Queries;

    public class ApplicationsController : ApiControllerBase
    {
        public IMembershipApplicationQueries MembershipApplicationQueries { get; set; }

        public IList<MembershipApplicationDto> Get(int organizationId)
        {
            return MembershipApplicationQueries.PendingByOrganizationId(organizationId);
        }

        [Transaction]
        public virtual void Delete(int organizationId, Guid id)
        {
            Dispatch(new RejectMembershipApplication {ApplicationId = id, InitiatorId = CurrentUserId});
        }

        [Transaction]
        public virtual void Post(int organizationId)
        {
            Dispatch(new ApplyForMembership {ApplicantId = CurrentUserId, OrganizationId = organizationId});
        }
    }
}
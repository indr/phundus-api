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

        public IList<MembershipApplicationDto> Get(int organization)
        {
            return MembershipApplicationQueries.PendingByOrganizationId(organization);
        }

        [Transaction]
        public virtual void Delete(int organization, Guid id)
        {
            Dispatch(new RejectMembershipApplication {ApplicationId = id, AdministratorId = CurrentUserId});
        }

        [Transaction]
        public virtual void Post(int organization)
        {
            Dispatch(new ApplyForMembership {UserId = CurrentUserId, OrganizationId = organization});
        }
    }
}
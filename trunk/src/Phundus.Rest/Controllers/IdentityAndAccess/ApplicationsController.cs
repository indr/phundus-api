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

        public IUserQueries UserQueries { get; set; }

        public IList<MembershipApplicationDto> Get(int organization)
        {
            return MembershipApplicationQueries.PendingByOrganizationId(organization);
        }

        [Transaction]
        public virtual void Delete(int organization, Guid id)
        {
            var user = UserQueries.ByEmail(Identity.Name);

            Dispatch(new RejectMembershipApplication {ApplicationId = id, AdministratorId = user.Id});
        }

        [Transaction]
        public virtual void Post(int organization)
        {
            var user = UserQueries.ByEmail(Identity.Name);

            Dispatch(new ApplyForMembership {UserId = user.Id, OrganizationId = organization});
        }
    }
}
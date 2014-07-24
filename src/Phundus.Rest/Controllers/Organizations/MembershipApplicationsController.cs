namespace Phundus.Rest.Controllers.Organizations
{
    using System;
    using System.Collections.Generic;
    using Castle.Transactions;
    using Core.IdentityAndAccessCtx.Queries;
    using Core.OrganizationAndMembershipCtx.Commands;
    using Core.OrganizationAndMembershipCtx.Queries;

    public class MembershipApplicationsController : ApiControllerBase
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

            Dispatch(new RejectMembershipRequest { RequestId = id, MemberId = user.Id });
        }

        [Transaction]
        public virtual void Patch(int organization, Guid id)
        {
            var user = UserQueries.ByEmail(Identity.Name);

            Dispatch(new ApproveMembershipRequest { RequestId = id, MemberId = user.Id });
        }

        [Transaction]
        public virtual void Post(int organization)
        {
            var user = UserQueries.ByEmail(Identity.Name);

            Dispatch(new ApplyForMembership {UserId = user.Id, OrganizationId = organization});
        }

        
    }
}
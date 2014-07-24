namespace Phundus.Rest.Controllers.Organizations
{
    using System;
    using Castle.Transactions;
    using Core.IdentityAndAccessCtx.Queries;
    using Core.OrganizationAndMembershipCtx.Commands;
    using Core.OrganizationAndMembershipCtx.Queries;

    public class MembershipApplicationsController : ApiControllerBase
    {
        public IMembershipApplicationQueries MembershipApplicationQueries { get; set; }

        public IUserQueries UserQueries { get; set; }

        public MembershipApplicationDtos Get(int organization)
        {
            return MembershipApplicationQueries.PendingByOrganizationId(organization);
        }

        [Transaction]
        public virtual void Post(int organization)
        {
            var user = UserQueries.ByEmail(Identity.Name);
                
            Dispatch(new ApplyForMembership {UserId = user.Id, OrganizationId = organization});
        }

        public void Delete(int organization, Guid id)
        {
            Dispatch(new RejectMembershipRequest {RequestId = id});
        }

        public void Patch(int organization, Guid id)
        {
            Dispatch(new ApproveMembershipRequest {RequestId = id});
        }
    }
}
namespace Phundus.Rest.Api.Organizations
{
    using System;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Organizations.Commands;

    [RoutePrefix("api/organizations/{organizationId}/members/{memberId}/locks")]
    public class OrganizationsMembersLocksController : ApiControllerBase
    {
        [POST("")]
        [Transaction]
        public virtual HttpResponseMessage Post(Guid organizationId, int memberId)
        {
            Dispatcher.Dispatch(new LockMember
            {
                InitiatorId = CurrentUserId.Id,
                MemberId = memberId,
                OrganizationId = organizationId
            });

            return CreateNoContentResponse();
        }

        [DELETE("")]
        [Transaction]
        public virtual HttpResponseMessage Delete(Guid organizationId, int memberId)
        {
            Dispatcher.Dispatch(new UnlockMember
            {
                InitiatorId = CurrentUserId.Id,
                MemberId = memberId,
                OrganizationId = organizationId
            });

            return CreateNoContentResponse();
        }
    }
}
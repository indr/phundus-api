﻿namespace Phundus.Rest.Api.Organizations
{
    using System;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Organizations.Commands;

    [RoutePrefix("api/organizations/{organizationId}/members/{memberId}/locks")]
    public class OrganizationsMembersLocksController : ApiControllerBase
    {
        [POST("")]
        [Transaction]
        public virtual void Post(Guid organizationId, int memberId)
        {
            Dispatcher.Dispatch(new LockMember
            {
                InitiatorId = CurrentUserId,
                MemberId = memberId,
                OrganizationId = organizationId
            });
        }

        [DELETE("")]
        [Transaction]
        public virtual void Delete(Guid organizationId, int memberId)
        {
            Dispatcher.Dispatch(new UnlockMember
            {
                InitiatorId = CurrentUserId,
                MemberId = memberId,
                OrganizationId = organizationId
            });
        }
    }
}
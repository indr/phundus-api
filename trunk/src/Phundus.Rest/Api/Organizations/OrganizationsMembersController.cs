namespace Phundus.Rest.Api.Organizations
{
    using System.Collections.Generic;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Organizations.Commands;
    using Core.IdentityAndAccess.Organizations.Repositories;
    using Core.IdentityAndAccess.Queries;

    [RoutePrefix("api/organizations/{organizationId}/members")]
    public class OrganizationsMembersController : ApiControllerBase
    {
        public IOrganizationRepository Organizations { get; set; }

        public IMembershipQueries MembershipQueries { get; set; }

        public IMemberQueries MemberQueries { get; set; }

        [GET("")]
        [Transaction]
        public virtual IList<MemberDto> Get(int organizationId)
        {
            return MemberQueries.ByOrganizationId(organizationId);
        }

        [POST("")]
        [Transaction]
        public virtual void Post(int organizationId, dynamic doc)
        {
            Dispatcher.Dispatch(new ApproveMembershipApplication
            {
                InitiatorId = CurrentUserId,
                ApplicationId = doc.applicationId
            });
        }

        [PUT("{memberId}")]
        [Transaction]
        public virtual void Put(int organizationId, int memberId, dynamic doc)
        {
            Dispatcher.Dispatch(new ChangeMembersRole
            {
                OrganizationId = organizationId,
                InitiatorId = CurrentUserId,
                MemberId = memberId,
                Role = doc.role
            });
        }
    }
}
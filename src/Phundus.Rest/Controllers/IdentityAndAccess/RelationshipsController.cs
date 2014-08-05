namespace Phundus.Rest.Controllers.IdentityAndAccess
{
    using Castle.Transactions;
    using Core.IdentityAndAccess.Queries;

    public class RelationshipsController : ApiControllerBase
    {
        public IRelationshipQueries RelationshipQueries { get; set; }

        [Transaction]
        public virtual RelationshipDto Get(int organization)
        {
            return RelationshipQueries.ByMemberIdForOrganizationId(CurrentUserId, organization);
        }
    }
}
namespace Phundus.Core.IdentityAndAccess.Queries
{
    public interface IRelationshipQueries
    {
        RelationshipDto ByMemberIdForOrganizationId(int memberId, int organizationId);
    }
}
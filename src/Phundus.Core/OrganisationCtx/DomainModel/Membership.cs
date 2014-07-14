namespace Phundus.Core.OrganisationCtx.DomainModel
{
    public class Membership
    {
        public Membership(int memberId)
        {
            MemberId = memberId;
        }

        public int MemberId { get; protected set; }
    }
}
namespace Phundus.Core.OrganisationCtx.DomainModel
{
    using Ddd;

    public class Membership
    {
        
    }

    public class MembershipRequest
    {
        public MembershipRequest(int memberId)
        {
            MemberId = memberId;
        }

        public int MemberId { get; protected set; }

        public Membership Approve()
        {
            EventPublisher.Publish(new MembershipRequestApproved());
        }

        public void Reject()
        {
            EventPublisher.Publish(new MembershipRequestRejected());
        }
    }
}
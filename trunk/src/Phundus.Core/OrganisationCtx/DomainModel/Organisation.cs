namespace Phundus.Core.OrganisationCtx.DomainModel
{
    using Ddd;
    using Iesi.Collections.Generic;

    public class Organisation
    {
        private ISet<MembershipRequest> _membershipRequests =  new HashedSet<MembershipRequest>();

        public virtual ISet<MembershipRequest> MembershipRequests
        {
            get { return _membershipRequests; }
            set { _membershipRequests = value; }
        }

        public void RequestMembershipFor(Member member)
        {
            _membershipRequests.Add(new MembershipRequest(member.Id));

            EventPublisher.Publish(new MembershipRequested());
        }
    }
}
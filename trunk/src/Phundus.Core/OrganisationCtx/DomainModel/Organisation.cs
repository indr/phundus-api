namespace Phundus.Core.OrganisationCtx.DomainModel
{
    using Ddd;
    using Iesi.Collections.Generic;

    public class Organisation
    {
        private ISet<Membership> _memberships =  new HashedSet<Membership>();

        public virtual ISet<Membership> Memberships
        {
            get { return _memberships; }
            set { _memberships = value; }
        }

        public void CreateMembership(Member member)
        {
            _memberships.Add(new Membership(member.Id));

            EventPublisher.Publish(new MembershipRequested());
        }
    }
}
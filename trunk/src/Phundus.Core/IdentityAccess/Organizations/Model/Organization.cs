namespace Phundus.IdentityAccess.Organizations.Model
{
    using System;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using Ddd;
    using Iesi.Collections.Generic;
    using Users.Model;

    public class Organization : Aggregate<Guid>
    {
        private DateTime _createDate = DateTime.UtcNow;
        private Guid _id = Guid.NewGuid();
        private ISet<Membership> _memberships = new HashedSet<Membership>();
        private string _name;
        private string _startpage;

        public Organization(Guid id, string name) : base(id)
        {
            _name = name;
        }

        public Organization(InitiatorGuid initiatorGuid, OrganizationGuid organizationGuid, string name)
            : base(organizationGuid.Id)
        {
            if (initiatorGuid == null) throw new ArgumentNullException("initiatorGuid");
            if (name == null) throw new ArgumentNullException("name");
            _name = name;
        }

        protected Organization()
        {
        }

        private OrganizationGuid OrganizationGuid
        {
            get { return new OrganizationGuid(base.Id); }
        }

        public virtual DateTime CreateDate
        {
            get { return _createDate; }
            protected set { _createDate = value; }
        }

        public virtual string Name
        {
            get { return _name; }
            protected set { _name = value; }
        }

        public virtual OrganizationPlan Plan { get; protected set; }

        public virtual string Url
        {
            get
            {
                if (Plan == OrganizationPlan.Free)
                    return "";

                return Name.ToFriendlyUrl();
            }
        }

        public virtual ISet<Membership> Memberships
        {
            get { return _memberships; }
            protected set { _memberships = value; }
        }

        public virtual string Address { get; set; }

        public virtual string Startpage
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_startpage))
                    return
                        String.Format(
                            "<p>Startseite der Organisation \"{0}\".</p><p>Diese Seite kann unter \"Verwaltung\" / \"Einstellungen\" angepasst werden.",
                            Name);
                return _startpage;
            }
            set { _startpage = value; }
        }

        public virtual string EmailAddress { get; set; }

        public virtual string Website { get; set; }

        public virtual string DocTemplateFileName { get; set; }

        public virtual MembershipApplication RequestMembership(InitiatorGuid initiatorGuid, Guid applicationId,
            User user)
        {
            var request = new MembershipApplication(applicationId, Id, user.UserGuid);

            EventPublisher.Publish(new MembershipApplicationFiled(initiatorGuid, OrganizationGuid, user.UserGuid));

            return request;
        }

        public virtual void ApproveMembershipRequest(UserGuid initiatorGuid, MembershipApplication application,
            Guid membershipId)
        {
            var membership = application.Approve(membershipId);
            membership.Organization = this;
            Memberships.Add(membership);

            EventPublisher.Publish(new MembershipApplicationApproved(initiatorGuid, OrganizationGuid,
                application.UserGuid));
        }

        public virtual void RejectMembershipRequest(UserGuid initiatorGuid, MembershipApplication application)
        {
            application.Reject();

            EventPublisher.Publish(new MembershipApplicationRejected(initiatorGuid, OrganizationGuid, application.UserGuid));
        }

        protected virtual Membership GetMembershipOfUser(User user)
        {
            var membership = Memberships.FirstOrDefault(p => p.UserGuid.Id == user.Guid);
            if (membership == null)
                throw new Exception("Membership not found");

            return membership;
        }

        public virtual void SetMembersRole(User member, Role role)
        {
            var membership = GetMembershipOfUser(member);
            membership.ChangeRole(role);
        }

        public virtual void LockMember(User member)
        {
            var membership = GetMembershipOfUser(member);
            membership.Lock();

            EventPublisher.Publish(new MemberLocked(Id, member.Id));
        }

        public virtual void UnlockMember(User member)
        {
            var membership = GetMembershipOfUser(member);
            membership.Unlock();

            EventPublisher.Publish(new MemberUnlocked(Id, member.Id));
        }

        public virtual void ChangeStartpage(UserGuid initiatorId, string startpage)
        {
            if (_startpage == startpage)
                return;

            Startpage = startpage;

            EventPublisher.Publish(new StartpageChanged());
        }
    }
}
namespace Phundus.IdentityAccess.Organizations.Model
{
    using System;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using Ddd;
    using IdentityAccess.Model;
    using Iesi.Collections.Generic;
    using Integration.IdentityAccess;
    using Users.Model;

    public class Organization : Aggregate<OrganizationId>
    {
        private ISet<MembershipApplication> _applications = new HashedSet<MembershipApplication>();
        private ContactDetails _contactDetails;
        private DateTime _establishedAtUtc = DateTime.UtcNow;
        private ISet<Membership> _memberships = new HashedSet<Membership>();
        private string _name;
        private Settings _settings = new Settings();
        private string _startpage;

        public Organization(OrganizationId organizationId, string name) : base(organizationId)
        {
            if (name == null) throw new ArgumentNullException("name");
            _name = name;
        }

        protected Organization()
        {
        }

        public virtual DateTime EstablishedAtUtc
        {
            get { return _establishedAtUtc; }
            protected set { _establishedAtUtc = value; }
        }

        public virtual string Name
        {
            get { return _name; }
            protected set { _name = value; }
        }

        public virtual OrganizationPlan Plan { get; protected set; }

        public virtual ContactDetails ContactDetails
        {
            get { return _contactDetails; }
            protected set { _contactDetails = value; }
        }

        public virtual string FriendlyUrl
        {
            get { return Name.ToFriendlyUrl(); }
        }

        public virtual ISet<Membership> Memberships
        {
            get { return _memberships; }
            protected set { _memberships = value; }
        }

        public virtual ISet<MembershipApplication> Applications
        {
            get { return _applications; }
            protected set { _applications = value; }
        }

        public virtual string Startpage
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_startpage))
                    return String.Format(
                        "<p>Startseite der Organisation \"{0}\".</p><p>Diese Seite kann unter \"Verwaltung\" / \"Einstellungen\" angepasst werden.",
                        Name);
                return _startpage;
            }
            set { _startpage = value; }
        }

        public virtual string DocTemplateFileName { get; set; }

        public virtual Settings Settings
        {
            get { return _settings; }
            protected set { _settings = value; }
        }

        public virtual MembershipApplication RequestMembership(InitiatorId initiatorId,
            MembershipApplicationId membershipApplicationId,
            User user)
        {
            if (Applications.FirstOrDefault(p => Equals(p.UserId, user.UserId)) != null)
                return null;

            var application = new MembershipApplication(membershipApplicationId.Id, Id.Id, user.UserId);
            Applications.Add(application);

            EventPublisher.Publish(new MembershipApplicationFiled(initiatorId, Id, user.UserId));

            return application;
        }

        public virtual void ApproveMembershipRequest(UserId initiatorId, MembershipApplication application,
            Guid membershipId)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (application == null) throw new ArgumentNullException("application");

            var membership = application.Approve(membershipId);
            membership.Organization = this;
            Memberships.Add(membership);

            EventPublisher.Publish(new MembershipApplicationApproved(initiatorId, Id,
                application.UserId));
        }

        public virtual void RejectMembershipRequest(UserId initiatorId, MembershipApplication application)
        {
            application.Reject();

            EventPublisher.Publish(new MembershipApplicationRejected(initiatorId, Id,
                application.UserId));
        }

        private Membership GetMembershipOfUser(User user)
        {
            return GetMembershipOfUser(user.UserId);
        }

        private Membership GetMembershipOfUser(UserId userId)
        {
            var membership = Memberships.FirstOrDefault(p => p.UserId.Id == userId.Id);
            if (membership == null)
                throw new Exception("Membership not found");

            return membership;
        }

        public virtual void SetMembersRole(User member, Role role)
        {
            var membership = GetMembershipOfUser(member);
            membership.ChangeRole(role);
        }

        public virtual void LockMember(Manager manager, UserId memberId)
        {
            var membership = GetMembershipOfUser(memberId);
            membership.Lock();

            EventPublisher.Publish(new MemberLocked(Id, memberId.Id));
        }

        public virtual void UnlockMember(Manager manager, UserId memberId)
        {
            var membership = GetMembershipOfUser(memberId);
            membership.Unlock();

            EventPublisher.Publish(new MemberUnlocked(Id, memberId.Id));
        }

        public virtual void ChangeMembersRecieveEmailNotificationOption(Manager manager, UserId memberId, bool value)
        {
            var membership = GetMembershipOfUser(memberId);
            membership.SetRecievesEmailNotification(value);

            EventPublisher.Publish(new MemberRecieveEmailNotificationOptionChanged(manager, Id, memberId, value));
        }
       
        public virtual void ChangeStartpage(UserId initiatorId, string startpage)
        {
            if (_startpage == startpage)
                return;

            Startpage = startpage;

            EventPublisher.Publish(new StartpageChanged());
        }

        public virtual void ChangeContactDetails(ContactDetails contactDetails)
        {
            if (contactDetails == null) throw new ArgumentNullException("contactDetails");
            if (Equals(_contactDetails, contactDetails))
                return;

            ContactDetails = contactDetails;

            EventPublisher.Publish(new OrganizationContactDetailsChanged());
        }

        public virtual void ChangeSettingPublicRental(Initiator initiator, bool value)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");

            if (_settings.PublicRental == value)
                return;

            _settings = new Settings(value);

            EventPublisher.Publish(new PublicRentalSettingChanged(initiator, Id, _settings.PublicRental));
        }

        public virtual void ChangePlan(Admin admin, OrganizationPlan plan)
        {
            if (admin == null) throw new ArgumentNullException("admin");

            if (Plan == plan)
                return;

            var oldPlan = Plan;
            Plan = plan;

            EventPublisher.Publish(new OrganizationPlanChanged(admin, Id, oldPlan, Plan));
        }
    }
}
namespace Phundus.IdentityAccess.Organizations.Model
{
    using System;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using Common.Eventing;
    using IdentityAccess.Model;
    using IdentityAccess.Model.Organizations;
    using IdentityAccess.Model.Users;
    using Iesi.Collections.Generic;
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

        public virtual void Rename(Manager manager, string name)
        {
            this.Name = name;

            EventPublisher.Publish(new OrganizationRenamed(manager, Id, name));
        }

        public virtual MembershipApplication ApplyForMembership(InitiatorId initiatorId,
            MembershipApplicationId membershipApplicationId,
            User user)
        {
            if (Applications.FirstOrDefault(p => Equals(p.UserId, user.UserId)) != null)
                return null;

            var application = new MembershipApplication(membershipApplicationId, Id, user.UserId);
            Applications.Add(application);

            EventPublisher.Publish(new MembershipApplicationFiled(initiatorId, Id, user.UserId));

            return application;
        }

        public virtual void ApproveMembershipApplication(UserId initiatorId, MembershipApplication application,
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

        public virtual void SetMembersRole(User member, MemberRole memberRole)
        {
            var membership = GetMembershipOfUser(member);
            membership.ChangeRole(memberRole);
        }

        public virtual void LockMember(Manager manager, UserId memberId)
        {
            var membership = GetMembershipOfUser(memberId);
            membership.Lock(manager);

            EventPublisher.Publish(new MemberLocked(Id, memberId.Id));
        }

        public virtual void UnlockMember(Manager manager, UserId memberId)
        {
            var membership = GetMembershipOfUser(memberId);
            membership.Unlock(manager);

            EventPublisher.Publish(new MemberUnlocked(Id, memberId.Id));
        }

        public virtual void ChangeMembersRecieveEmailNotificationOption(Manager manager, UserId memberId, bool value)
        {
            var membership = GetMembershipOfUser(memberId);
            membership.SetRecievesEmailNotification(value);

            var count = Memberships.Count(p => p.MemberRole == MemberRole.Manager && p.RecievesEmailNotifications);
            if (count == 0)
                throw new NoManagerWithReceivesEmailNotificationException();

            EventPublisher.Publish(new MemberRecieveEmailNotificationOptionChanged(manager, Id, memberId, value));
        }

        public virtual void ChangeStartpage(Manager manager, string startpage)
        {
            if (_startpage == startpage)
                return;

            Startpage = startpage;

            EventPublisher.Publish(new StartpageChanged(manager, Id, startpage));
        }

        public virtual void ChangeContactDetails(Manager manager, ContactDetails contactDetails)
        {
            if (contactDetails == null) throw new ArgumentNullException("contactDetails");
            if (Equals(_contactDetails, contactDetails))
                return;

            ContactDetails = contactDetails;

            EventPublisher.Publish(new OrganizationContactDetailsChanged(manager, Id, contactDetails.Line1,
                contactDetails.Line2, contactDetails.Street, contactDetails.Postcode, contactDetails.City,
                contactDetails.PhoneNumber, contactDetails.EmailAddress, contactDetails.Website));
        }

        public virtual void ChangeSettingPublicRental(Manager manager, bool value)
        {
            AssertionConcern.AssertArgumentNotNull(manager, "Manager must be provided.");

            if (_settings.PublicRental == value)
                return;

            _settings = new Settings(value, _settings.PdfTemplateFileName, _settings.OrderReceivedText);

            EventPublisher.Publish(new PublicRentalSettingChanged(manager, Id, _settings.PublicRental));
        }

        public virtual void ChangePlan(Admin admin, OrganizationPlan plan)
        {
            AssertionConcern.AssertArgumentNotNull(admin, "Admin must be provided.");

            if (Plan == plan)
                return;

            var oldPlan = Plan;
            Plan = plan;

            EventPublisher.Publish(new OrganizationPlanChanged(admin, Id, oldPlan, Plan));
        }

        public virtual void SetPdfTemplateFileName(Manager manager, string pdfTemplateFileName)
        {
            AssertionConcern.AssertArgumentNotNull(manager, "Manager must be provided.");

            if (_settings.PdfTemplateFileName == pdfTemplateFileName)
                return;

            _settings = new Settings(_settings.PublicRental, pdfTemplateFileName, _settings.OrderReceivedText);

            EventPublisher.Publish(new PdfTemplateChanged(manager, Id, _settings.PdfTemplateFileName));
        }

        public virtual void ChangeEmailTemplate(Manager manager, string orderReceivedText)
        {
            AssertionConcern.AssertArgumentNotNull(manager, "Manager must be provided.");

            if (_settings.OrderReceivedText == orderReceivedText)
                return;

            _settings = new Settings(_settings.PublicRental, _settings.PdfTemplateFileName, orderReceivedText);

            EventPublisher.Publish(new EmailTemplateChanged(manager, Id, _settings.OrderReceivedText));
        }
    }
}
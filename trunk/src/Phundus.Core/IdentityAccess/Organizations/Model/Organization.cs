namespace Phundus.IdentityAccess.Organizations.Model
{
    using System;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using Ddd;
    using IdentityAccess.Model;
    using Iesi.Collections.Generic;
    using Users.Model;
    using ApplicationId = Common.Domain.Model.ApplicationId;

    public class Organization : Aggregate<Guid>
    {
        private ISet<MembershipApplication> _applications = new HashedSet<MembershipApplication>();
        private ContactDetails _contactDetails;
        private DateTime _establishedAtUtc = DateTime.UtcNow;
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

        public virtual ISet<MembershipApplication> Applications
        {
            get { return _applications; }
            protected set { _applications = value; }
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

        public virtual MembershipApplication RequestMembership(InitiatorGuid initiatorGuid, ApplicationId applicationId,
            User user)
        {
            if (Applications.FirstOrDefault(p => Equals(p.UserGuid, user.UserGuid)) != null)
                return null;

            var application = new MembershipApplication(applicationId.Id, Id, user.UserGuid);
            Applications.Add(application);

            EventPublisher.Publish(new MembershipApplicationFiled(initiatorGuid, OrganizationGuid, user.UserGuid));

            return application;
        }

        [Obsolete]
        public virtual MembershipApplication RequestMembership(InitiatorGuid initiatorGuid, Guid applicationId,
            User user)
        {
            return RequestMembership(initiatorGuid, new ApplicationId(applicationId), user);
        }

        public virtual void ApproveMembershipRequest(UserGuid initiatorGuid, MembershipApplication application,
            Guid membershipId)
        {
            if (initiatorGuid == null) throw new ArgumentNullException("initiatorGuid");
            if (application == null) throw new ArgumentNullException("application");

            var membership = application.Approve(membershipId);
            membership.Organization = this;
            Memberships.Add(membership);

            EventPublisher.Publish(new MembershipApplicationApproved(initiatorGuid, OrganizationGuid,
                application.UserGuid));
        }

        public virtual void RejectMembershipRequest(UserGuid initiatorGuid, MembershipApplication application)
        {
            application.Reject();

            EventPublisher.Publish(new MembershipApplicationRejected(initiatorGuid, OrganizationGuid,
                application.UserGuid));
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

            EventPublisher.Publish(new MemberLocked(Id, member.Guid));
        }

        public virtual void UnlockMember(User member)
        {
            var membership = GetMembershipOfUser(member);
            membership.Unlock();

            EventPublisher.Publish(new MemberUnlocked(Id, member.Guid));
        }

        public virtual void ChangeStartpage(UserGuid initiatorId, string startpage)
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
    }
}
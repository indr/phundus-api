﻿namespace Phundus.IdentityAccess.Organizations.Model
{
    using System;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using Ddd;
    using IdentityAccess.Model;
    using Iesi.Collections.Generic;
    using Users.Model;

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

        public Organization(InitiatorId initiatorId, OrganizationGuid organizationGuid, string name)
            : base(organizationGuid.Id)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
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

        public virtual string DocTemplateFileName { get; set; }

        public virtual MembershipApplication RequestMembership(InitiatorId initiatorId, MembershipApplicationId membershipApplicationId,
            User user)
        {
            if (Applications.FirstOrDefault(p => Equals(p.UserGuid, user.UserGuid)) != null)
                return null;

            var application = new MembershipApplication(membershipApplicationId.Id, Id, user.UserGuid);
            Applications.Add(application);

            EventPublisher.Publish(new MembershipApplicationFiled(initiatorId, OrganizationGuid, user.UserGuid));

            return application;
        }

        [Obsolete]
        public virtual MembershipApplication RequestMembership(InitiatorId initiatorId, Guid applicationId,
            User user)
        {
            return RequestMembership(initiatorId, new MembershipApplicationId(applicationId), user);
        }

        public virtual void ApproveMembershipRequest(UserGuid initiatorId, MembershipApplication application,
            Guid membershipId)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (application == null) throw new ArgumentNullException("application");

            var membership = application.Approve(membershipId);
            membership.Organization = this;
            Memberships.Add(membership);

            EventPublisher.Publish(new MembershipApplicationApproved(initiatorId, OrganizationGuid,
                application.UserGuid));
        }

        public virtual void RejectMembershipRequest(UserGuid initiatorId, MembershipApplication application)
        {
            application.Reject();

            EventPublisher.Publish(new MembershipApplicationRejected(initiatorId, OrganizationGuid,
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
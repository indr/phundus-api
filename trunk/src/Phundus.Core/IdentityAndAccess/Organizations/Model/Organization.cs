namespace Phundus.Core.IdentityAndAccess.Organizations.Model
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Ddd;
    using Iesi.Collections.Generic;
    using Users.Model;

    public class Organization : EntityBase
    {
        private Guid _guid = Guid.NewGuid();
        private DateTime _createDate = DateTime.UtcNow;
        private ISet<Membership> _memberships = new HashedSet<Membership>();
        private string _name;
        private string _startpage;

        public Organization(int id)
            : base(id)
        {
        }

        public Organization(string name)
        {
            _name = name;
        }

        protected Organization()
        {
        }

        public virtual Guid Guid
        {
            get {return _guid; }
            protected set { _guid = value; }
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

                // http://stackoverflow.com/questions/37809/how-do-i-generate-a-friendly-url-in-c
                // http://stackoverflow.com/questions/2161684/transform-title-into-dashed-url-friendly-string

                var url = Name.ToLowerInvariant();
                url = Regex.Replace(url, " ", "-");
                return Regex.Replace(url, @"[^A-Za-zÄÖÜäöü0-9\-\._~]+", "");
            }
        }

        public virtual ISet<Membership> Memberships
        {
            get { return _memberships; }
            protected set { _memberships = value; }
        }

        public virtual string Address { get; set; }

        public virtual string Coordinate { get; set; }

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

        public virtual MembershipApplication RequestMembership(Guid requestId, User user)
        {
            var request = new MembershipApplication(
                requestId,
                Id,
                Guid,
                user);

            EventPublisher.Publish(new MembershipApplicationFiled(Id, user.Id, Guid));

            return request;
        }

        public virtual void ApproveMembershipRequest(MembershipApplication application, Guid membershipId)
        {
            var membership = application.Approve(membershipId);
            membership.Organization = this;
            Memberships.Add(membership);

            EventPublisher.Publish(new MembershipApplicationApproved(Id, application.UserId, Guid));
        }

        public virtual void RejectMembershipRequest(MembershipApplication application)
        {
            application.Reject();

            EventPublisher.Publish(new MembershipApplicationRejected(Id, application.UserId, Guid));
        }

        protected virtual Membership GetMembershipOfUser(User user)
        {
            var membership = Memberships.FirstOrDefault(p => p.UserId == user.Id);
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
    }
}
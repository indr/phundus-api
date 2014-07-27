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
        private DateTime _createDate = DateTime.Now;
        private ISet<Membership> _memberships = new HashedSet<Membership>();
        private string _startpage;

        public Organization()
        {
        }

        public Organization(int id) : base(id)
        {
        }

        public virtual DateTime CreateDate
        {
            get { return _createDate; }
            protected set { _createDate = value; }
        }

        public virtual string Name { get; set; }

        public virtual string Url
        {
            get
            {
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
            set { _memberships = value; }
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

        public virtual MembershipRequest RequestMembership(Guid requestId, User user)
        {
            var request = new MembershipRequest(
                requestId,
                Id,
                user);

            EventPublisher.Publish(new MembershipRequested());

            return request;
        }

        public virtual Membership ApproveMembershipRequest(MembershipRequest request, Guid membershipId)
        {
            var membership = request.Approve(membershipId);

            EventPublisher.Publish(new MembershipRequestApproved());

            return membership;
        }

        public virtual void RejectMembershipRequest(MembershipRequest request)
        {
            request.Reject();

            EventPublisher.Publish(new MembershipRequestRejected());
        }

        public virtual void SetMembersRole(User administrator, User member, int roleId)
        {
            var membership = Memberships.FirstOrDefault(p => p.MemberId == member.Id);
            if (membership == null)
                throw new Exception("Membership not found");
            
            membership.Role = roleId;
        }
    }
}
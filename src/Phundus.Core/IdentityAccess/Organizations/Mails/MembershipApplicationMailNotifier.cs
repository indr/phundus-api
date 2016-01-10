namespace Phundus.Core.IdentityAndAccess.Organizations.Mails
{
    using System;
    using System.Linq;
    using Ddd;
    using IdentityAccess.Organizations.Mails;
    using Infrastructure;
    using Infrastructure.Gateways;
    using Integration.IdentityAccess;
    using Model;
    using Queries;

    public class MembershipApplicationMailNotifier : BaseMail, ISubscribeTo<MembershipApplicationFiled>,
        ISubscribeTo<MembershipApplicationApproved>, ISubscribeTo<MembershipApplicationRejected>,
        ISubscribeTo<MemberLocked>, ISubscribeTo<MemberUnlocked>
    {
        private readonly IOrganizationQueries _organizationQueries;
        private readonly IUserQueries _userQueries;
        private readonly IMembersWithRole _memberWithRole;

        public MembershipApplicationMailNotifier(IMailGateway mailGateway, IOrganizationQueries organizationQueries,
            IUserQueries userQueries, IMembersWithRole memberWithRole) : base(mailGateway)
        {
            if (organizationQueries == null) throw new ArgumentNullException("organizationQueries");
            if (userQueries == null) throw new ArgumentNullException("userQueries");
            if (memberWithRole == null) throw new ArgumentNullException("memberWithRole");


            _organizationQueries = organizationQueries;
            _userQueries = userQueries;
            _memberWithRole = memberWithRole;
        }

        public void Handle(MemberLocked @event)
        {
            var user = _userQueries.GetById(@event.MemberId);
            var organization = _organizationQueries.GetById(@event.OrganizationId);

            Model = new
            {
                User = user,
                Organization = organization,
                Urls = new Urls(Config.ServerUrl),
                Admins = Config.FeedbackRecipients
            };

            Send(user.Email, Templates.MemberLockedSubject,
                null, Templates.MemberLockedBodyHtml);
        }

        public void Handle(MembershipApplicationApproved @event)
        {
            var user = _userQueries.GetById(@event.UserId);
            var organization = _organizationQueries.GetById(@event.OrganizationId);

            Model = new
            {
                User = user,
                Organization = organization,
                Urls = new Urls(Config.ServerUrl),
                Admins = Config.FeedbackRecipients
            };

            Send(user.Email, Templates.MembershipApplicationApprovedSubject,
                null, Templates.MembershipApplicationApprovedBodyHtml);
        }

        public void Handle(MembershipApplicationFiled @event)
        {
            var user = _userQueries.GetById(@event.UserId);
            var organization = _organizationQueries.GetById(@event.OrganizationId);
            var chiefs = _memberWithRole.Manager(@event.OrganizationId);

            var recipients = chiefs.Select(p => p.EmailAddress).ToList();

            Model = new
            {
                User = user,
                Organization = organization,
                Urls = new Urls(Config.ServerUrl),
                Admins = Config.FeedbackRecipients
            };

            Send(String.Join(",", recipients), Templates.MembershipApplicationFiledSubject,
                null, Templates.MembershipApplicationFiledBodyHtml);
        }

        public void Handle(MembershipApplicationRejected @event)
        {
            var user = _userQueries.GetById(@event.UserId);
            var organization = _organizationQueries.GetById(@event.OrganizationId);

            Model = new
            {
                User = user,
                Organization = organization,
                Urls = new Urls(Config.ServerUrl),
                Admins = Config.FeedbackRecipients
            };

            Send(user.Email, Templates.MembershipApplicationRejectedSubject,
                null, Templates.MembershipApplicationRejectedBodyHtml);
        }

        public void Handle(MemberUnlocked @event)
        {
            var user = _userQueries.GetById(@event.MemberId);
            var organization = _organizationQueries.GetById(@event.OrganizationId);

            Model = new
            {
                User = user,
                Organization = organization,
                Urls = new Urls(Config.ServerUrl),
                Admins = Config.FeedbackRecipients
            };

            Send(user.Email, Templates.MemberUnlockedSubject,
                null, Templates.MemberUnlockedBodyHtml);
        }
    }
}
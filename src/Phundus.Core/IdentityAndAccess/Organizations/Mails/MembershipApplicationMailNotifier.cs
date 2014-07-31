namespace Phundus.Core.IdentityAndAccess.Organizations.Mails
{
    using System;
    using System.Linq;
    using Ddd;
    using Infrastructure;
    using Model;
    using Queries;

    public class MembershipApplicationMailNotifier : BaseMail, ISubscribeTo<MembershipApplicationFiled>,
        ISubscribeTo<MembershipApplicationApproved>, ISubscribeTo<MembershipApplicationRejected>, ISubscribeTo<MemberLocked>, ISubscribeTo<MemberUnlocked>
    {
        public IMemberInRoleQueries MemberInRoleQueries { get; set; }

        public IOrganizationQueries OrganizationQueries { get; set; }

        public IUserQueries UserQueries { get; set; }

        public void Handle(MembershipApplicationApproved @event)
        {
            var user = UserQueries.ById(@event.UserId);
            var organization = OrganizationQueries.ById(@event.OrganizationId);

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
            var user = UserQueries.ById(@event.UserId);
            var organization = OrganizationQueries.ById(@event.OrganizationId);
            var chiefs = MemberInRoleQueries.Chiefs(@event.OrganizationId);

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
            var user = UserQueries.ById(@event.UserId);
            var organization = OrganizationQueries.ById(@event.OrganizationId);

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

        public void Handle(MemberLocked @event)
        {
            var user = UserQueries.ById(@event.MemberId);
            var organization = OrganizationQueries.ById(@event.OrganizationId);

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

        public void Handle(MemberUnlocked @event)
        {
            var user = UserQueries.ById(@event.MemberId);
            var organization = OrganizationQueries.ById(@event.OrganizationId);

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
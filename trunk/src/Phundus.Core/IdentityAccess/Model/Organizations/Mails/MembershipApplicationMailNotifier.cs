namespace Phundus.IdentityAccess.Organizations.Mails
{
    using System;
    using System.Linq;
    using Common;
    using Common.Eventing;
    using Common.Mailing;
    using IdentityAccess.Model.Organizations.Mails;   
    using Integration.IdentityAccess;
    using Model;
    using Projections;

    public class MembershipApplicationMailNotifier : BaseMail, ISubscribeTo<MembershipApplicationFiled>,
        ISubscribeTo<MembershipApplicationApproved>, ISubscribeTo<MembershipApplicationRejected>,
        ISubscribeTo<MemberLocked>, ISubscribeTo<MemberUnlocked>
    {
        private readonly IMembersWithRole _memberWithRole;
        private readonly IOrganizationQueries _organizationQueries;
        private readonly IUsersQueries _usersQueries;

        public MembershipApplicationMailNotifier(IMailGateway mailGateway, IOrganizationQueries organizationQueries,
            IUsersQueries usersQueries, IMembersWithRole memberWithRole) : base(mailGateway)
        {
            if (organizationQueries == null) throw new ArgumentNullException("organizationQueries");
            if (usersQueries == null) throw new ArgumentNullException("usersQueries");
            if (memberWithRole == null) throw new ArgumentNullException("memberWithRole");


            _organizationQueries = organizationQueries;
            _usersQueries = usersQueries;
            _memberWithRole = memberWithRole;
        }

        public void Handle(MemberLocked @event)
        {
            var user = _usersQueries.GetByGuid(@event.UserGuid);
            var organization = _organizationQueries.GetById(@event.OrganizationGuid);

            Model = new
            {
                User = user,
                Organization = organization,
                Urls = new Urls(Config.ServerUrl),
                Admins = Config.FeedbackRecipients
            };

            Send(user.EmailAddress, Templates.MemberLockedSubject,
                null, Templates.MemberLockedBodyHtml);
        }

        public void Handle(MembershipApplicationApproved @event)
        {
            var user = _usersQueries.GetByGuid(@event.UserGuid);
            var organization = _organizationQueries.GetById(@event.OrganizationGuid);

            Model = new
            {
                User = user,
                Organization = organization,
                Urls = new Urls(Config.ServerUrl),
                Admins = Config.FeedbackRecipients
            };

            Send(user.EmailAddress, Templates.MembershipApplicationApprovedSubject,
                null, Templates.MembershipApplicationApprovedBodyHtml);
        }

        public void Handle(MembershipApplicationFiled @event)
        {
            var user = _usersQueries.GetByGuid(@event.UserGuid);
            var organization = _organizationQueries.GetById(@event.OrganizationGuid);
            var managers = _memberWithRole.Manager(@event.OrganizationGuid, true);

            var recipients = managers.Select(p => p.EmailAddress).ToList();

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
            var user = _usersQueries.GetByGuid(@event.UserGuid);
            var organization = _organizationQueries.GetById(@event.OrganizationGuid);

            Model = new
            {
                User = user,
                Organization = organization,
                Urls = new Urls(Config.ServerUrl),
                Admins = Config.FeedbackRecipients
            };

            Send(user.EmailAddress, Templates.MembershipApplicationRejectedSubject,
                null, Templates.MembershipApplicationRejectedBodyHtml);
        }

        public void Handle(MemberUnlocked @event)
        {
            var user = _usersQueries.GetByGuid(@event.UserGuid);
            var organization = _organizationQueries.GetById(@event.OrganizationGuid);

            Model = new
            {
                User = user,
                Organization = organization,
                Urls = new Urls(Config.ServerUrl),
                Admins = Config.FeedbackRecipients
            };

            Send(user.EmailAddress, Templates.MemberUnlockedSubject,
                null, Templates.MemberUnlockedBodyHtml);
        }
    }
}
namespace Phundus.IdentityAccess.Organizations.Mails
{
    using System;
    using System.Linq;
    using Common;
    using Common.Eventing;
    using Common.Mailing;
    using Common.Notifications;
    using IdentityAccess.Model.Organizations.Mails;
    using Integration.IdentityAccess;
    using Model;
    using Projections;

    public class MembershipApplicationMailNotifier : BaseMail,
        //ISubscribeTo<MembershipApplicationFiled>,
        IConsumes<MembershipApplicationFiled>,
        ISubscribeTo<MembershipApplicationApproved>,
        ISubscribeTo<MembershipApplicationRejected>,
        ISubscribeTo<MemberLocked>,
        ISubscribeTo<MemberUnlocked>
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

        public void Handle(MemberLocked e)
        {
            var user = _usersQueries.GetById(e.UserGuid);
            var organization = _organizationQueries.GetById(e.OrganizationGuid);

            Model = new
            {
                User = user,
                Organization = organization,
                Urls = new Urls(Config.ServerUrl),
                Admins = Config.FeedbackRecipients
            };

            Send(e.OccuredOnUtc, user.EmailAddress, Templates.MemberLockedSubject,
                null, Templates.MemberLockedBodyHtml);
        }

        public void Handle(MembershipApplicationApproved e)
        {
            var user = _usersQueries.GetById(e.UserGuid);
            var organization = _organizationQueries.GetById(e.OrganizationGuid);

            Model = new
            {
                User = user,
                Organization = organization,
                Urls = new Urls(Config.ServerUrl),
                Admins = Config.FeedbackRecipients
            };

            Send(e.OccuredOnUtc, user.EmailAddress, Templates.MembershipApplicationApprovedSubject,
                null, Templates.MembershipApplicationApprovedBodyHtml);
        }

        public void Handle(MembershipApplicationFiled e)
        {
            var user = _usersQueries.GetById(e.UserGuid);
            var organization = _organizationQueries.GetById(e.OrganizationGuid);
            var managers = _memberWithRole.Manager(e.OrganizationGuid, true);

            var recipients = managers.Select(p => p.EmailAddress).ToList();

            Model = new
            {
                User = user,
                Organization = organization,
                Urls = new Urls(Config.ServerUrl),
                Admins = Config.FeedbackRecipients
            };

            Send(e.OccuredOnUtc, String.Join(",", recipients), Templates.MembershipApplicationFiledSubject,
                null, Templates.MembershipApplicationFiledBodyHtml);
        }

        public void Handle(MembershipApplicationRejected e)
        {
            var user = _usersQueries.GetById(e.UserGuid);
            var organization = _organizationQueries.GetById(e.OrganizationGuid);

            Model = new
            {
                User = user,
                Organization = organization,
                Urls = new Urls(Config.ServerUrl),
                Admins = Config.FeedbackRecipients
            };

            Send(e.OccuredOnUtc, user.EmailAddress, Templates.MembershipApplicationRejectedSubject,
                null, Templates.MembershipApplicationRejectedBodyHtml);
        }

        public void Handle(MemberUnlocked e)
        {
            var user = _usersQueries.GetById(e.UserGuid);
            var organization = _organizationQueries.GetById(e.OrganizationGuid);

            Model = new
            {
                User = user,
                Organization = organization,
                Urls = new Urls(Config.ServerUrl),
                Admins = Config.FeedbackRecipients
            };

            Send(e.OccuredOnUtc, user.EmailAddress, Templates.MemberUnlockedSubject,
                null, Templates.MemberUnlockedBodyHtml);
        }
    }
}
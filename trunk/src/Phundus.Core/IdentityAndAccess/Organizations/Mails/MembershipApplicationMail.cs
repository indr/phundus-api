namespace Phundus.Core.IdentityAndAccess.Organizations.Mails
{
    using System;
    using System.Linq;
    using Ddd;
    using Infrastructure;
    using Model;
    using Queries;

    /// <summary>
    /// Sendet ein E-Mail an die Administratoren der Organisation um über die Beitrittsanfrage zu
    /// informieren.
    /// </summary>
    public class MembershipApplicationMail : BaseMail, ISubscribeTo<MembershipRequested>
    {
        private new const string Subject = @"Mitgliedschaft wurde beantragt";
        private new const string TextBody = @"Hallo";
        private new const string HtmlBody = null;

        public MembershipApplicationMail() : base(Subject, TextBody, HtmlBody)
        {
        }

        public IUserQueries UserQueries { get; set; }

        public IMemberInRoleQueries MemberInRoleQueries { get; set; }

        public void Handle(MembershipRequested @event)
        {
            var user = UserQueries.ById(@event.UserId);
            var chiefs = MemberInRoleQueries.Chiefs(@event.OrganizationId);

            var recipients = chiefs.Select(p => p.EmailAddress).ToList();

            Model = new
            {
                Urls = new Urls(Config.ServerUrl),
                Admins = Config.FeedbackRecipients
            };

            Send(recipients);
        }
    }
}
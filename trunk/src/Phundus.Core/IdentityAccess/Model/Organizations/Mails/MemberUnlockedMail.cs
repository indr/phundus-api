namespace Phundus.IdentityAccess.Model.Organizations.Mails
{
    using System;
    using Common.Mailing;
    using Common.Notifications;
    using IdentityAccess.Organizations.Model;
    using Integration.IdentityAccess;

    public class MemberUnlockedMail : IConsumes<MemberUnlocked>
    {
        private readonly IMessageFactory _factory;
        private readonly IMailGateway _gateway;
        private readonly IUsersQueries _userQueries;

        public MemberUnlockedMail(IMessageFactory factory, IMailGateway gateway, IUsersQueries userQueries)
        {
            _factory = factory;
            _gateway = gateway;
            _userQueries = userQueries;
        }

        public void Handle(MemberUnlocked e)
        {
            var user = _userQueries.FindById(e.UserGuid);
            var model = new Model
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailAddress = user.EmailAddress
            };

            var message = _factory.MakeMessage(model, Templates.MemberUnlockedSubject, null,
                Templates.MemberUnlockedHtml);
            message.To.Add(user.EmailAddress);

            _gateway.Send(e.OccuredOnUtc, message);
        }

        public class Model : MailModel
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string EmailAddress { get; set; }
        }
    }
}
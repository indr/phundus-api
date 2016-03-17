namespace Phundus.IdentityAccess.Model.Organizations.Mails
{
    using Application;
    using Common.Eventing;
    using Common.Mailing;
    using IdentityAccess.Organizations.Model;

    public class MemberUnlockedMail : ISubscribeTo<MemberUnlocked>
    {
        private readonly IMessageFactory _factory;
        private readonly IMailGateway _gateway;
        private readonly IUserQueryService _userQueries;

        public MemberUnlockedMail(IMessageFactory factory, IMailGateway gateway, IUserQueryService userQueries)
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
namespace Phundus.IdentityAccess.Model.Organizations.Mails
{
    using Application;
    using Common.Eventing;
    using Common.Mailing;
    using IdentityAccess.Organizations.Model;

    public class MemberLockedMail : ISubscribeTo<MemberLocked>
    {
        private readonly IMessageFactory _factory;
        private readonly IMailGateway _gateway;
        private readonly IUsersQueries _userQueries;

        public MemberLockedMail(IMessageFactory factory, IMailGateway gateway, IUsersQueries userQueries)
        {
            _factory = factory;
            _gateway = gateway;
            _userQueries = userQueries;
        }

        public void Handle(MemberLocked e)
        {
            var user = _userQueries.FindById(e.UserGuid);
            var model = new Model
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailAddress = user.EmailAddress
            };

            var message = _factory.MakeMessage(model, Templates.MemberLockedSubject, null,
                Templates.MemberLockedHtml);
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
namespace Phundus.IdentityAccess.Model.Users.Mails
{
    using Common.Eventing;
    using Common.Mailing;
    using IdentityAccess.Users.Model;
    using Resources;

    public class AccountValidationMail : ISubscribeTo<UserSignedUp>
    {
        private readonly IMessageFactory _factory;
        private readonly IMailGateway _gateway;

        public AccountValidationMail(IMessageFactory factory, IMailGateway gateway)
        {
            _factory = factory;
            _gateway = gateway;
        }

        public void Handle(UserSignedUp e)
        {
            var model = new Model
            {
                FirstName = e.FirstName,
                LastName = e.LastName,
                ValidationKey = e.ValidationKey
            };

            var message = _factory.MakeMessage(model, Templates.AccountValidationSubject, null,
                Templates.AccountValidationHtml);
            message.To.Add(e.EmailAddress);

            _gateway.Send(e.OccuredOnUtc, message);
        }

        public class Model : MailModel
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string ValidationKey { get; set; }
        }
    }
}
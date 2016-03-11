namespace Phundus.IdentityAccess.Model.Users.Mails
{
    using Common.Mailing;
    using Common.Notifications;

    public class NewPasswordMail : IConsumes<PasswordResetted>
    {
        private readonly IMessageFactory _factory;
        private readonly IMailGateway _gateway;

        public NewPasswordMail(IMessageFactory factory, IMailGateway gateway)
        {
            _factory = factory;
            _gateway = gateway;
        }

        public void Handle(PasswordResetted e)
        {
            var model = new Model
            {
                FirstName = e.FirstName,
                LastName = e.LastName,
                Password = e.NewPassword
            };

            var message = _factory.MakeMessage(model, Resources.Templates.NewPasswordSubject, null,
                Resources.Templates.NewPasswordHtml);
            message.To.Add(e.EmailAddress);

            _gateway.Send(e.OccuredOnUtc, message);
        }

        public class Model : MailModel
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Password { get; set; }
        }
    }
}
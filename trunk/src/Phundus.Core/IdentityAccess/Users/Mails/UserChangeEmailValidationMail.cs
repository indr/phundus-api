namespace Phundus.IdentityAccess.Users.Mails
{
    using System;
    using Ddd;
    using Infrastructure;
    using Infrastructure.Gateways;
    using Model;
    using Repositories;

    public class UserChangeEmailValidationMail : BaseMail, ISubscribeTo<UserEmailAddressChangeRequested>
    {
        private readonly IUserRepository _userRepository;

        public UserChangeEmailValidationMail(IMailGateway mailGateway, IUserRepository userRepository) : base(mailGateway)
        {
            if (userRepository == null) throw new ArgumentNullException("userRepository");
            _userRepository = userRepository;
        }
       
        public void Handle(UserEmailAddressChangeRequested @event)
        {
            var user = _userRepository.FindByGuid(@event.UserGuid);
            if (user == null)
                return;

            Model = new
            {
                Urls = new Urls(Config.ServerUrl),
                User = user,
                Admins = Config.FeedbackRecipients
            };

            Send(user.Account.RequestedEmail, Templates.UserChangeEmailValidationSubject, null,
                Templates.UserChangeEmailValidationHtml);
        }
    }
}
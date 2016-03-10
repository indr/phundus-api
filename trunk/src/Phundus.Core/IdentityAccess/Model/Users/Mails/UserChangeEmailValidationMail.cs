namespace Phundus.IdentityAccess.Users.Mails
{
    using System;
    using Common;
    using Common.Eventing;
    using Common.Mailing;
    using IdentityAccess.Model.Users;
    using IdentityAccess.Model.Users.Mails;    
    using Model;

    public class UserChangeEmailValidationMail : BaseMail,
        ISubscribeTo<UserEmailAddressChangeRequested>
    {
        private readonly IUserRepository _userRepository;

        public UserChangeEmailValidationMail(IMailGateway mailGateway, IUserRepository userRepository) : base(mailGateway)
        {
            if (userRepository == null) throw new ArgumentNullException("userRepository");
            _userRepository = userRepository;
        }
       
        public void Handle(UserEmailAddressChangeRequested e)
        {
            var user = _userRepository.FindByGuid(e.UserGuid);
            if (user == null)
                return;

            Model = new
            {
                Urls = new Urls(Config.ServerUrl),
                User = user,
                Admins = Config.FeedbackRecipients
            };

            Send(e.OccuredOnUtc, user.Account.RequestedEmail, Templates.UserChangeEmailValidationSubject, null,
                Templates.UserChangeEmailValidationHtml);
        }
    }
}
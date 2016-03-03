namespace Phundus.IdentityAccess.Application
{
    using System;
    using Common.Commanding;
    using Common.Mailing;
    using Model.Users;
    using Users.Mails;

    public class ResetPassword
    {
        public ResetPassword(string username)
        {
            Username = username;
        }

        public string Username { get; private set; }
    }

    public class ResetPasswordHandler : IHandleCommand<ResetPassword>
    {
        public IMailGateway MailGateway { get; set; }

        public IUserRepository UserRepository { get; set; }

        public void Handle(ResetPassword command)
        {
            var user = UserRepository.FindByEmailAddress(command.Username);
            if (user == null)
                throw new Exception("Die E-Mail-Adresse konnte nicht gefunden werden.");
            var password = user.Account.ResetPassword();

            new UserResetPasswordMail(MailGateway).For(user, password).Send(user);
        }
    }
}
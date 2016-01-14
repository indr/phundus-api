namespace Phundus.IdentityAccess.Users.Commands
{
    using System;
    using Cqrs;
    using IdentityAccess.Users.Repositories;
    using Infrastructure.Gateways;
    using Mails;

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
            UserRepository.Update(user);

            new UserResetPasswordMail(MailGateway).For(user, password).Send(user);
        }
    }
}
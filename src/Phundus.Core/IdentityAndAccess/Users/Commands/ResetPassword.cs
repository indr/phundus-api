namespace Phundus.Core.IdentityAndAccessCtx.Commands
{
    using System;
    using Cqrs;
    using Mails;
    using Repositories;

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
        public IUserRepository UserRepository { get; set; }

        public void Handle(ResetPassword command)
        {
            var user = UserRepository.FindByEmail(command.Username);
            if (user == null)
                throw new Exception("Die E-Mail-Adresse konnte nicht gefunden werden.");
            var password = user.SiteMembership.ResetPassword();
            UserRepository.Update(user);
            
            new UserResetPasswordMail().For(user, password).Send(user);
        }
    }
}
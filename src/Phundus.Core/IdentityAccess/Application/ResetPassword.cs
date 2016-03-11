namespace Phundus.IdentityAccess.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Eventing;
    using Common.Mailing;    
    using Model.Users;    

    public class ResetPassword : ICommand
    {
        public ResetPassword(string username)
        {
            if (username == null) throw new ArgumentNullException("username");
            Username = username;
        }

        public string Username { get; protected set; }
    }

    public class ResetPasswordHandler : IHandleCommand<ResetPassword>
    {
        private readonly IUserRepository _userRepository;        

        public ResetPasswordHandler(IUserRepository userRepository)
        {
            if (userRepository == null) throw new ArgumentNullException("userRepository");            
            _userRepository = userRepository;            
        }

        [Transaction]
        public void Handle(ResetPassword command)
        {
            var user = _userRepository.FindByEmailAddress(command.Username);
            if (user == null)
                throw new Exception("Die E-Mail-Adresse konnte nicht gefunden werden.");
            var password = user.Account.ResetPassword();

            EventPublisher.Publish(new PasswordResetted(user.UserId, user.FirstName, user.LastName, user.EmailAddress, password));
        }
    }
}
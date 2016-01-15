namespace Phundus.IdentityAccess.Users.Commands
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
    using Repositories;

    public class ChangePassword
    {
        public ChangePassword(InitiatorGuid initiatorGuid, string oldPassword, string newPassword)
        {
            if (initiatorGuid == null) throw new ArgumentNullException("initiatorGuid");
            if (oldPassword == null) throw new ArgumentNullException("oldPassword");
            if (newPassword == null) throw new ArgumentNullException("newPassword");

            InitiatorGuid = initiatorGuid;
            UserGuid = new UserGuid(initiatorGuid.Id);
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }

        public InitiatorGuid InitiatorGuid { get; protected set; }
        public UserGuid UserGuid { get; protected set; }
        public string OldPassword { get; protected set; }
        public string NewPassword { get; protected set; }
    }

    public class ChangePasswordHandler : IHandleCommand<ChangePassword>
    {
        public IUserRepository UserRepository { get; set; }

        public void Handle(ChangePassword command)
        {
            var user = UserRepository.GetById(command.UserGuid);

            user.Account.ChangePassword(command.OldPassword, command.NewPassword);
        }
    }
}
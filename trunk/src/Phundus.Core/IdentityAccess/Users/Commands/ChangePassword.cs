namespace Phundus.IdentityAccess.Users.Commands
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
    using Repositories;

    public class ChangePassword
    {
        public ChangePassword(InitiatorId initiatorId, string oldPassword, string newPassword)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (oldPassword == null) throw new ArgumentNullException("oldPassword");
            if (newPassword == null) throw new ArgumentNullException("newPassword");

            InitiatorId = initiatorId;
            UserGuid = new UserGuid(initiatorId.Id);
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public UserGuid UserGuid { get; protected set; }
        public string OldPassword { get; protected set; }
        public string NewPassword { get; protected set; }
    }

    public class ChangePasswordHandler : IHandleCommand<ChangePassword>
    {
        public IUserRepository UserRepository { get; set; }

        public void Handle(ChangePassword command)
        {
            var user = UserRepository.GetByGuid(command.UserGuid);

            user.Account.ChangePassword(command.OldPassword, command.NewPassword);
        }
    }
}
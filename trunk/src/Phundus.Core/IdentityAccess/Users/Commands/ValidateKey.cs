namespace Phundus.Core.IdentityAndAccess.Users.Commands
{
    using System;
    using Cqrs;
    using Repositories;

    public class ValidateKey
    {
        public ValidateKey(string key)
        {
            if (key == null) throw new ArgumentNullException("key");

            Key = key;
        }

        public string Key { get; set; }
    }

    public class ValidateKeyHandler : IHandleCommand<ValidateKey>
    {
        private readonly IUserRepository _userRepository;

        public ValidateKeyHandler(IUserRepository userRepository)
        {
            if (userRepository == null) throw new ArgumentNullException("userRepository");

            _userRepository = userRepository;
        }

        public void Handle(ValidateKey command)
        {
            var user = _userRepository.FindByValidationKey(command.Key);
            user.Account.ValidateKey(command.Key);
        }
    }
}
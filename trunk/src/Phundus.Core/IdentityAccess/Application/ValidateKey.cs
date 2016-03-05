namespace Phundus.IdentityAccess.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Model.Users;

    public class ValidateKey : ICommand
    {
        public ValidateKey(string validationKey)
        {
            if (validationKey == null) throw new ArgumentNullException("validationKey");
            ValidationKey = validationKey;
        }

        public string ValidationKey { get; protected set; }
    }

    public class ValidateKeyHandler : IHandleCommand<ValidateKey>
    {
        private readonly IUserRepository _userRepository;

        public ValidateKeyHandler(IUserRepository userRepository)
        {
            if (userRepository == null) throw new ArgumentNullException("userRepository");
            _userRepository = userRepository;
        }

        [Transaction]
        public void Handle(ValidateKey command)
        {
            var user = _userRepository.FindByValidationKey(command.ValidationKey);

            user.Account.ValidateKey(command.ValidationKey);
        }
    }
}
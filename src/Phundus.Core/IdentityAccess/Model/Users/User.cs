namespace Phundus.IdentityAccess.Users.Model
{
    using System;
    using Common.Domain.Model;
    using Common.Eventing;
    using IdentityAccess.Model.Users;

    public class User : EntityBase
    {
        private Account _account;
        private string _city;
        private string _firstName;
        private int? _jsNumber;
        private string _lastName;
        private string _phoneNumber;
        private string _postcode;
        private UserRole _role;
        private string _street;
        private UserId _userId;

        public User(UserId userId, string emailAddress, string password, string firstName, string lastName,
            string street,
            string postcode, string city, string phonePhone, int? jsNumber)
        {
            if (userId == null) throw new ArgumentNullException("userId");

            _userId = userId;
            _firstName = firstName;
            _lastName = lastName;
            _street = street;
            _postcode = postcode;
            _city = city;
            _phoneNumber = phonePhone;
            _jsNumber = jsNumber;

            _account = new Account();
            _account.User = this;
            _account.Email = emailAddress;
            _account.SetPassword(password);
            _account.GenerateValidationKey();

            _role = UserRole.User;
        }

        protected User()
        {
        }

        public virtual UserId UserId
        {
            get { return _userId; }
            protected set { _userId = value; }
        }

        public virtual string FirstName
        {
            get { return _firstName; }
            protected set { _firstName = value; }
        }

        public virtual string LastName
        {
            get { return _lastName; }
            protected set { _lastName = value; }
        }

        public virtual string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        public virtual string EmailAddress
        {
            get { return Account.Email; }
        }

        public virtual Account Account
        {
            get { return _account; }
            set { _account = value; }
        }

        public virtual UserRole Role
        {
            get { return _role; }
            protected set { _role = value; }
        }

        public virtual string Street
        {
            get { return _street; }
            protected set { _street = value; }
        }

        public virtual string Postcode
        {
            get { return _postcode; }
            protected set { _postcode = value; }
        }

        public virtual string City
        {
            get { return _city; }
            protected set { _city = value; }
        }

        public virtual string PhoneNumber
        {
            get { return _phoneNumber; }
            protected set { _phoneNumber = value; }
        }

        public virtual int? JsNumber
        {
            get { return _jsNumber; }
            set
            {
                if (value.HasValue && ((value > 9999999) || (value < 1)))
                    throw new ArgumentOutOfRangeException("value", "Die J+S-Nummer muss sechs- oder siebenstellig sein.");
                _jsNumber = value;
            }
        }

        public virtual bool IsLocked
        {
            get { return Account.IsLockedOut; }
        }

        public virtual void ChangeRole(Admin admin, UserRole userRole)
        {
            if (Role == userRole)
                return;

            var oldRole = Role;
            Role = userRole;

            EventPublisher.Publish(new UserRoleChanged(admin, this, oldRole, Role));
        }

        public virtual void Approve(Admin admin)
        {
            if (!Account.Approve())
                return;

            EventPublisher.Publish(new UserApproved(admin.UserId, UserId));
        }

        public virtual void ChangeEmailAddress(UserId initiatorId, string password, string newEmailAddress)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (newEmailAddress == null) throw new ArgumentNullException("newEmailAddress");

            Account.ChangeEmailAddress(initiatorId, password, newEmailAddress);
        }

        public virtual void Lock(Initiator initiator)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");

            Account.Lock(initiator);
        }

        public virtual void Unlock(Initiator initiator)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");

            Account.Unlock(initiator);
        }

        public virtual void ChangeAddress(Initiator initiator, string firstName, string lastName, string street,
            string postcode, string city, string phoneNumber)
        {
            Apply(new UserAddressChanged(initiator, UserId, firstName, lastName, street, postcode, city, phoneNumber));
        }

        private void When(UserAddressChanged e)
        {
            FirstName = e.FirstName;
            LastName = e.LastName;
            Street = e.Street;
            Postcode = e.Postcode;
            City = e.City;
            PhoneNumber = e.PhoneNumber;
        }

        private void Apply<TDomainEvent>(TDomainEvent e) where TDomainEvent : DomainEvent
        {
            When((dynamic)e);
            EventPublisher.Publish(e);
        }
    }
}
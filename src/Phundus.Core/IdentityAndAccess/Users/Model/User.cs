namespace Phundus.Core.IdentityAndAccess.Users.Model
{
    using System;
    using Common.Domain.Model;
    using Ddd;

    public class User : EntityBase
    {
        private Guid _guid = Guid.NewGuid();
        private Account _account;
        private string _firstName;
        private int? _jsNumber;
        private string _lastName;
        private UserRole _role;
        private string _street;
        private string _postcode;
        private string _city;
        private string _mobileNumber;

        public User() : this(0)
        {
        }

        public User(int id) : this(id, 0)
        {
        }

        public User(int id, int version) : base(id, version)
        {
            _firstName = "";
            _lastName = "";
            _account = new Account();
            _account.User = this;
            _role = UserRole.User;
        }

        public User(string emailAddress, string password, string firstName, string lastName, string street, string postcode, string city, string mobilePhone, int? jsNumber)
        {
            _firstName = firstName;
            _lastName = lastName;
            _street = street;
            _postcode = postcode;
            _city = city;
            _mobileNumber = mobilePhone;
            _jsNumber = jsNumber;

            _account = new Account();
            _account.User = this;
            _account.Email = emailAddress;
            _account.Password = password;
            _account.GenerateValidationKey();

            _role = UserRole.User;
        }

        public virtual Guid Guid
        {
            get { return _guid; }
            protected set { _guid = value; }
        }

        public virtual string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public virtual string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
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
            set { _street = value; }
        }

        public virtual string Postcode
        {
            get { return _postcode; }
            set { _postcode = value; }
        }

        public virtual string City
        {
            get { return _city; }
            set { _city = value; }
        }

        public virtual string MobileNumber
        {
            get { return _mobileNumber; }
            set { _mobileNumber = value; }
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

        public virtual string DisplayName
        {
            get { return String.Format("{0} {1}", FirstName, LastName); }
        }

        public virtual void ChangeRole(User initiator, UserRole userRole)
        {
            if (Role == userRole)
                return;

            var oldRole = Role;
            Role = userRole;

            EventPublisher.Publish(new UserRoleChanged(initiator, this, oldRole, Role));
        }
    }
}
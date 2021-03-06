namespace Phundus.Specs.Services.Entities
{
    using System;

    public class User
    {
        private string _password;

        public string Username
        {
            get { return EmailAddress; }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                OldPassword = _password;
                _password = value;
            }
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public Guid UserId { get; set; }
        public string City { get; set; }
        public string MobilePhone { get; set; }
        public string Street { get; set; }
        public string Postcode { get; set; }
        public string RequestedEmailAddress { get; set; }

        public Guid StoreId { get; set; }
        public string OldPassword { get; set; }

        public Guid Id
        {
            get { return UserId; }
        }

        public override string ToString()
        {
            return String.Format("[Id={0}, EmailAddress={1}, FirstName={2}, LastName={3}]",
                new object[] {UserId.ToString("D"), EmailAddress, FirstName, LastName});
        }
    }
}
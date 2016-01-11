namespace Phundus.Specs.Services.Entities
{
    using System;
    using System.Globalization;

    public class User
    {
        private string _password;
        public string Username { get { return EmailAddress; } }

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
        public Guid Guid { get; set; }
        public string City { get; set; }
        public string MobilePhone { get; set; }
        public string Street { get; set; }
        public string Postcode { get; set; }
        public string RequestedEmailAddress { get; set; }
        public int Id { get; set; }
        public Guid StoreId { get; set; }
        public string OldPassword { get; set; }

        public override string ToString()
        {
            return String.Format("[Id={0}, Guid={1}, EmailAddress={2}, FirstName={3}, LastName={4}]",
                new object[] {Id.ToString(CultureInfo.InvariantCulture), Guid.ToString("D"), EmailAddress, FirstName, LastName});
        }
    }
}
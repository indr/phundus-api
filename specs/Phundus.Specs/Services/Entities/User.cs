namespace Phundus.Specs.Services.Entities
{
    using System;

    public class User
    {
        public string Username { get { return EmailAddress; } }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public Guid Guid { get; set; }
        public string City { get; set; }
        public string MobilePhone { get; set; }
        public string Street { get; set; }
        public string Postcode { get; set; }
        public string RequestedEmailAddress { get; set; }

        public override string ToString()
        {
            return String.Format("[Guid={0}, EmailAddress={1}, FirstName={2}, LastName={3}]",
                new object[] {Guid.ToString("D"), EmailAddress, FirstName, LastName});
        }
    }
}
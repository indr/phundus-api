namespace Phundus.Specs.Services.Entities
{
    using System;

    public class User
    {
        public string Username { get; set; }
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
    }
}
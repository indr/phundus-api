namespace Phundus.Specs.Services.Entities
{
    using System;

    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public Guid Guid { get; set; }
    }
}
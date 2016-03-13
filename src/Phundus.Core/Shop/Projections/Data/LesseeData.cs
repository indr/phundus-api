namespace Phundus.Shop.Projections
{
    using System;

    public class LesseeData
    {
        internal LesseeData(Guid lesseeId, string name, string postalAddress, string phoneNumber, string emailAddress)
        {
            LesseeId = lesseeId;
            Name = name;
            PostalAddress = postalAddress;
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddress;
        }

        public Guid LesseeId { get; private set; }
        public string Name { get; private set; }
        public string PostalAddress { get; private set; }
        public string PhoneNumber { get; private set; }
        public string EmailAddress { get; private set; }
    }
}
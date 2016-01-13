namespace Phundus.Shop.Queries.QueryModels
{
    using System;
    using Integration.Shop;

    public class LesseeViewRow : ILessee
    {
        internal LesseeViewRow(Guid lesseeGuid, string name, string address, string phoneNumber, string emailAddress)
        {
            LesseeGuid = lesseeGuid;
            Name = name;
            Address = address;
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddress;
        }

        public Guid LesseeGuid { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string PhoneNumber { get; private set; }
        public string EmailAddress { get; private set; }
    }
}
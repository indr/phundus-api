namespace Phundus.Integration.Shop
{
    using System;

    public interface ILessee
    {
        Guid LesseeGuid { get; }
        string Name { get; }
        string Address { get; }
        string PhoneNumber { get; }
        string EmailAddress { get; }
    }
}
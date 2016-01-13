namespace Phundus.Integration.Shop
{
    using System;

    public interface ILessor
    {
        Guid LessorGuid { get; }
        int LessorType { get; }
        string Name { get; }
        string Address { get; }
        string PhoneNumber { get; }
        string EmailAddress { get; }
    }
}
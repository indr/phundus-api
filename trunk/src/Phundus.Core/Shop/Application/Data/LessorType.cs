namespace Phundus.Shop.Application
{
    using System;

    public enum LessorType
    {
        Organization,
        User
    }

    public static class LessorTypeConvertor
    {
        public static LessorType From(string value)
        {
            value = value == null ? null : value.ToLowerInvariant();
            if (value == "organization")
                return LessorType.Organization;
            if (value == "user")
                return LessorType.User;

            throw new ArgumentOutOfRangeException("value", "Unknown lessor type " + value);
        }

        public static string ToLowerString(this LessorType lessorType)
        {
            return lessorType.ToString().ToLowerInvariant();
        }
    }
}
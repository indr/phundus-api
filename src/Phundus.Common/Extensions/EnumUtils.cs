namespace Phundus.Common.Extensions
{
    using System;

    public abstract class EnumHelper
    {
        public static T Parse<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
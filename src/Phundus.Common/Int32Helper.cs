namespace Phundus.Common
{
    using System;

    public static class Int32Helper
    {
        public static Guid ToGuid(this Int32 value)
        {
            return new Guid(value.ToString("D32"));
        }
    }
}
namespace Phundus.Common.Domain.Model
{
    using System;

    public enum OwnerType
    {
        Unknown,
        Organization,
        User,

        [Obsolete]
        Adapted
    }
}
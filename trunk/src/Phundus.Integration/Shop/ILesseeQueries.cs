namespace Phundus.Integration.Shop
{
    using System;

    public interface ILesseeQueries
    {
        ILessee GetByGuid(Guid lesseeGuid);
    }
}
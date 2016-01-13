namespace Phundus.Integration.Shop
{
    using System;
    using System.Collections.Generic;

    public interface ILessorQueries
    {
        ILessor GetByGuid(Guid lessorGuid);
        IList<ILessor> Query();
    }
}
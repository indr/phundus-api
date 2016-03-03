namespace Phundus.Common.Projecting
{
    using System;

    public interface IProjection
    {
        void Reset();
        Type GetEntityType();
    }
}
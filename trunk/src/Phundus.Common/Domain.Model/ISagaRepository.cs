namespace Phundus.Common.Domain.Model
{
    using System;

    public interface ISagaRepository
    {
        TSaga GetById<TSaga>(Guid sagaId) where TSaga : ISaga, new();
        void Save(ISaga saga);
    }
}
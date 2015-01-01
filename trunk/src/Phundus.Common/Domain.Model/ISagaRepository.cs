namespace Phundus.Common.Domain.Model
{
    using System;

    public interface ISagaRepository
    {
        TSaga FindById<TSaga>(Guid id);
        void Save(ISaga saga);
    }
}
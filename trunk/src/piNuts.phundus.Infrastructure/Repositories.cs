namespace Phundus.Infrastructure
{
    using System;

    public interface IRepository<T>
    {
        T ById(object id);
        void Remove(T entity);
        T Add(T entity);

        [Obsolete]
        void Update(T entity);
    }
}
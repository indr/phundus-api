namespace Phundus.Infrastructure
{
    public interface IRepository<T>
    {
        T ById(object id);
        void Remove(T entity);
        T Add(T entity);

        void Update(T entity);
    }
}
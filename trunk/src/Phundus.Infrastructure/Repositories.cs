namespace Phundus.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        void Remove(T entity);
        T Add(T entity);
    }
}
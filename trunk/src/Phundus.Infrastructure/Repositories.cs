namespace Phundus.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        T FindById(object id);

        void Remove(T entity);
        T Add(T entity);

        void Update(T entity);
    }
}
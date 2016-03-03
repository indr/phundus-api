namespace Phundus.Common.Domain.Model
{
    public interface IRepository<T> where T : class
    {
        void Remove(T entity);
        T Add(T entity);
    }
}
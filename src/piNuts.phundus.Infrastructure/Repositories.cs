namespace piNuts.phundus.Infrastructure
{
    public interface IRepository<T>
    {
        T Get(object id);
        void Delete(T entity);
        T Save(T entity);
        T SaveOrUpdate(T entity);
        void Update(T entity);
    }
}
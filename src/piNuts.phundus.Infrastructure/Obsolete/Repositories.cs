namespace piNuts.phundus.Infrastructure.Obsolete
{
    using NHibernate.Criterion;

    public interface IRepository<T>
    {
        T Get(object id);
        T Load(object id);
        void Delete(T entity);
        void DeleteAll();
        void DeleteAll(DetachedCriteria where);
        T Save(T entity);
        T SaveOrUpdate(T entity);
        T SaveOrUpdateCopy(T entity);
        void Update(T entity);
        T FindFirst(params Order[] orders);
    }
}
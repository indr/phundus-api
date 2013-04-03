namespace piNuts.phundus.Infrastructure.NHibernate
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using global::NHibernate.Criterion;
    using global::NHibernate.SqlCommand;

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

        ICollection<T> FindAll(Order order, params ICriterion[] criteria);

        ICollection<T> FindAll(DetachedCriteria criteria, params Order[] orders);

        ICollection<T> FindAll(DetachedCriteria criteria, int firstResult, int maxResults, params Order[] orders);

        ICollection<T> FindAll(Order[] orders, params ICriterion[] criteria);

        ICollection<T> FindAll(params ICriterion[] criteria);

        ICollection<T> FindAll(int firstResult, int numberOfResults, params ICriterion[] criteria);

        ICollection<T> FindAll(int firstResult, int numberOfResults, Order selectionOrder, params ICriterion[] criteria);

        ICollection<T> FindAll(int firstResult, int numberOfResults, Order[] selectionOrder,
                               params ICriterion[] criteria);

        ICollection<T> FindAll(string namedQuery, params Parameter[] parameters);

        ICollection<T> FindAll(int firstResult, int numberOfResults, string namedQuery, params Parameter[] parameters);

        T FindOne(params ICriterion[] criteria);

        T FindOne(DetachedCriteria criteria);

        T FindOne(string namedQuery, params Parameter[] parameters);

        T FindFirst(DetachedCriteria criteria, params Order[] orders);

        T FindFirst(params Order[] orders);

        object ExecuteStoredProcedure(string sp_name, params Parameter[] parameters);

        ICollection<T2> ExecuteStoredProcedure<T2>(Converter<IDataReader, T2> converter, string sp_name,
                                                   params Parameter[] parameters);

        bool Exists(DetachedCriteria criteria);

        bool Exists();

        long Count(DetachedCriteria criteria);

        long Count();

        ProjT ReportOne<ProjT>(DetachedCriteria criteria, ProjectionList projectionList);

        ProjT ReportOne<ProjT>(ProjectionList projectionList, params ICriterion[] criteria);

        ICollection<ProjT> ReportAll<ProjT>(ProjectionList projectionList);

        ICollection<ProjT> ReportAll<ProjT>(DetachedCriteria criteria, ProjectionList projectionList);

        ICollection<ProjT> ReportAll<ProjT>(DetachedCriteria criteria, ProjectionList projectionList,
                                            params Order[] orders);

        ICollection<ProjT> ReportAll<ProjT>(ProjectionList projectionList, params ICriterion[] criterion);

        ICollection<ProjT> ReportAll<ProjT>(ProjectionList projectionList, Order[] orders, params ICriterion[] criteria);

        ICollection<ProjT> ReportAll<ProjT>(ProjectionList projectionList, params Order[] orders);

        ICollection<ProjT> ReportAll<ProjT>(ProjectionList projectionList, bool distinctResults);

        ICollection<ProjJ> ReportAll<ProjJ>(string namedQuery, params Parameter[] parameters);

        DetachedCriteria CreateDetachedCriteria();

        DetachedCriteria CreateDetachedCriteria(string alias);

        T Create();
    }
}
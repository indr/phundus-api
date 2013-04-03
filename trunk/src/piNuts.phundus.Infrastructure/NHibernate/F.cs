using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace piNuts.phundus.Infrastructure.NHibernate
{
    using System.Collections;
    using System.Data;
    using global::NHibernate;
    using global::NHibernate.Connection;
    using global::NHibernate.Criterion;
    using global::NHibernate.Engine;
    using global::NHibernate.SqlCommand;
    using global::NHibernate.Transform;

    public abstract class RepositoryImplBase<T>
    {
        static readonly Order[] NullOrderArray = (Order[]) null;
        Type concreteType;

        public Type ConcreteType
        {
            get { return this.concreteType ?? typeof (T); }
            set { this.concreteType = value; }
        }

        protected abstract DisposableAction<ISession> ActionToBePerformedOnSessionUsedForDbFetches { get; }

        protected abstract ISessionFactory SessionFactory { get; }

        static RepositoryImplBase()
        {
        }

        public FutureValue<T> FutureGet(object id)
        {
            return new FutureValue<T>(id, FutureValueOptions.NullIfNotFound);
        }

        public FutureValue<T> FutureLoad(object id)
        {
            return new FutureValue<T>(id, FutureValueOptions.ThrowIfNotFound);
        }

        public DetachedCriteria CreateDetachedCriteria()
        {
            return DetachedCriteria.For<T>();
        }

        public DetachedCriteria CreateDetachedCriteria(string alias)
        {
            return DetachedCriteria.For<T>(alias);
        }

        public ICollection<T> FindAll(params ICriterion[] criteria)
        {
            return this.FindAll(RepositoryImplBase<T>.NullOrderArray, criteria);
        }

        public ICollection<T> FindAll(Order order, params ICriterion[] criteria)
        {
            return this.FindAll(new Order[1]
                                    {
                                        order
                                    }, criteria);
        }

        public ICollection<T> FindAll(Order[] orders, params ICriterion[] criteria)
        {
            using (DisposableAction<ISession> usedForDbFetches = this.ActionToBePerformedOnSessionUsedForDbFetches)
                return
                    (ICollection<T>)
                    RepositoryHelper<T>.CreateCriteriaFromArray(usedForDbFetches.Value, criteria, orders).List<T>();
        }

        public ICollection<T> FindAll(DetachedCriteria criteria, params Order[] orders)
        {
            using (DisposableAction<ISession> usedForDbFetches = this.ActionToBePerformedOnSessionUsedForDbFetches)
                return
                    (ICollection<T>)
                    RepositoryHelper<T>.GetExecutableCriteria(usedForDbFetches.Value, criteria,
                                                              new RepositoryHelper<T>.CriteriaCreator(
                                                                  this.CreateCriteria), orders).List<T>();
        }

        public ICollection<T> FindAll(DetachedCriteria criteria, int firstResult, int maxResults, params Order[] orders)
        {
            using (DisposableAction<ISession> usedForDbFetches = this.ActionToBePerformedOnSessionUsedForDbFetches)
            {
                ICriteria executableCriteria = RepositoryHelper<T>.GetExecutableCriteria(usedForDbFetches.Value,
                                                                                         criteria,
                                                                                         new RepositoryHelper<T>.
                                                                                             CriteriaCreator(
                                                                                             this.CreateCriteria),
                                                                                         orders);
                executableCriteria.SetFirstResult(firstResult).SetMaxResults(maxResults);
                return (ICollection<T>) executableCriteria.List<T>();
            }
        }

        public ICollection<T> FindAll(int firstResult, int numberOfResults, params ICriterion[] criteria)
        {
            return this.FindAll(firstResult, numberOfResults, RepositoryImplBase<T>.NullOrderArray, criteria);
        }

        public ICollection<T> FindAll(int firstResult, int numberOfResults, Order selectionOrder,
                                      params ICriterion[] criteria)
        {
            return this.FindAll(firstResult, numberOfResults, new Order[1]
                                                                  {
                                                                      selectionOrder
                                                                  }, criteria);
        }

        public ICollection<T> FindAll(int firstResult, int numberOfResults, Order[] selectionOrder,
                                      params ICriterion[] criteria)
        {
            using (DisposableAction<ISession> usedForDbFetches = this.ActionToBePerformedOnSessionUsedForDbFetches)
            {
                ICriteria criteriaFromArray = RepositoryHelper<T>.CreateCriteriaFromArray(usedForDbFetches.Value,
                                                                                          criteria, selectionOrder);
                criteriaFromArray.SetFirstResult(firstResult).SetMaxResults(numberOfResults);
                return (ICollection<T>) criteriaFromArray.List<T>();
            }
        }

        public ICollection<T> FindAll(string namedQuery, params Parameter[] parameters)
        {
            using (DisposableAction<ISession> usedForDbFetches = this.ActionToBePerformedOnSessionUsedForDbFetches)
                return
                    (ICollection<T>)
                    RepositoryHelper<T>.CreateQuery(usedForDbFetches.Value, namedQuery, parameters).List<T>();
        }

        public ICollection<T> FindAll(int firstResult, int numberOfResults, string namedQuery,
                                      params Parameter[] parameters)
        {
            using (DisposableAction<ISession> usedForDbFetches = this.ActionToBePerformedOnSessionUsedForDbFetches)
            {
                IQuery query = RepositoryHelper<T>.CreateQuery(usedForDbFetches.Value, namedQuery, parameters);
                query.SetFirstResult(firstResult).SetMaxResults(numberOfResults);
                return (ICollection<T>) query.List<T>();
            }
        }

        public T FindOne(params ICriterion[] criteria)
        {
            using (DisposableAction<ISession> usedForDbFetches = this.ActionToBePerformedOnSessionUsedForDbFetches)
                return
                    (T)
                    RepositoryHelper<T>.CreateCriteriaFromArray(usedForDbFetches.Value, criteria, new Order[0])
                                       .UniqueResult<T>();
        }

        public T FindOne(DetachedCriteria criteria)
        {
            using (DisposableAction<ISession> usedForDbFetches = this.ActionToBePerformedOnSessionUsedForDbFetches)
                return
                    (T)
                    RepositoryHelper<T>.GetExecutableCriteria(usedForDbFetches.Value, criteria,
                                                              new RepositoryHelper<T>.CriteriaCreator(
                                                                  this.CreateCriteria), new Order[0]).UniqueResult<T>();
        }

        public T FindOne(string namedQuery, params Parameter[] parameters)
        {
            using (DisposableAction<ISession> usedForDbFetches = this.ActionToBePerformedOnSessionUsedForDbFetches)
                return
                    (T)
                    RepositoryHelper<T>.CreateQuery(usedForDbFetches.Value, namedQuery, parameters).UniqueResult<T>();
        }

        public T FindFirst(params Order[] orders)
        {
            return this.FindFirst((DetachedCriteria) null, orders);
        }

        public T FindFirst(DetachedCriteria criteria, params Order[] orders)
        {
            using (DisposableAction<ISession> usedForDbFetches = this.ActionToBePerformedOnSessionUsedForDbFetches)
            {
                ICriteria executableCriteria = RepositoryHelper<T>.GetExecutableCriteria(usedForDbFetches.Value,
                                                                                         criteria,
                                                                                         new RepositoryHelper<T>.
                                                                                             CriteriaCreator(
                                                                                             this.CreateCriteria),
                                                                                         orders);
                executableCriteria.SetFirstResult(0);
                executableCriteria.SetMaxResults(1);
                return (T) executableCriteria.UniqueResult();
            }
        }

        public ProjT ReportOne<ProjT>(DetachedCriteria criteria, ProjectionList projectionList)
        {
            using (DisposableAction<ISession> usedForDbFetches = this.ActionToBePerformedOnSessionUsedForDbFetches)
                return
                    RepositoryImplBase<T>.DoReportOne<ProjT>(
                        RepositoryHelper<T>.GetExecutableCriteria(usedForDbFetches.Value, criteria,
                                                                  new RepositoryHelper<T>.CriteriaCreator(
                                                                      this.CreateCriteria), new Order[0]),
                        projectionList);
        }

        public ProjT ReportOne<ProjT>(ProjectionList projectionList, params ICriterion[] criteria)
        {
            using (DisposableAction<ISession> usedForDbFetches = this.ActionToBePerformedOnSessionUsedForDbFetches)
                return
                    RepositoryImplBase<T>.DoReportOne<ProjT>(
                        RepositoryHelper<T>.CreateCriteriaFromArray(usedForDbFetches.Value, criteria, new Order[0]),
                        projectionList);
        }

        static ProjT DoReportOne<ProjT>(ICriteria criteria, ProjectionList projectionList)
        {
            RepositoryImplBase<T>.BuildProjectionCriteria<ProjT>(criteria, (IProjection) projectionList, false);
            return criteria.UniqueResult<ProjT>();
        }

        public ICollection<ProjT> ReportAll<ProjT>(ProjectionList projectionList)
        {
            using (DisposableAction<ISession> usedForDbFetches = this.ActionToBePerformedOnSessionUsedForDbFetches)
                return
                    RepositoryImplBase<T>.DoReportAll<ProjT>(
                        RepositoryHelper<T>.GetExecutableCriteria(usedForDbFetches.Value, (DetachedCriteria) null,
                                                                  new Order[0]), projectionList);
        }

        public ICollection<ProjT> ReportAll<ProjT>(ProjectionList projectionList, params Order[] orders)
        {
            using (DisposableAction<ISession> usedForDbFetches = this.ActionToBePerformedOnSessionUsedForDbFetches)
                return
                    RepositoryImplBase<T>.DoReportAll<ProjT>(
                        RepositoryHelper<T>.GetExecutableCriteria(usedForDbFetches.Value, (DetachedCriteria) null,
                                                                  new RepositoryHelper<T>.CriteriaCreator(
                                                                      this.CreateCriteria), orders), projectionList);
        }

        public ICollection<ProjT> ReportAll<ProjT>(ProjectionList projectionList, bool distinctResults)
        {
            using (DisposableAction<ISession> usedForDbFetches = this.ActionToBePerformedOnSessionUsedForDbFetches)
                return
                    RepositoryImplBase<T>.DoReportAll<ProjT>(
                        RepositoryHelper<T>.GetExecutableCriteria(usedForDbFetches.Value, (DetachedCriteria) null,
                                                                  new RepositoryHelper<T>.CriteriaCreator(
                                                                      this.CreateCriteria), new Order[0]),
                        projectionList, distinctResults);
        }

        public ICollection<ProjT> ReportAll_original<ProjT>(DetachedCriteria criteria, ProjectionList projectionList)
        {
            using (DisposableAction<ISession> usedForDbFetches = this.ActionToBePerformedOnSessionUsedForDbFetches)
                return
                    RepositoryImplBase<T>.DoReportAll<ProjT>(
                        RepositoryHelper<T>.GetExecutableCriteria(usedForDbFetches.Value, criteria,
                                                                  new RepositoryHelper<T>.CriteriaCreator(
                                                                      this.CreateCriteria), new Order[0]),
                        projectionList);
        }

        public ICollection<ProjT> ReportAll<ProjT>(DetachedCriteria criteria, ProjectionList projectionList)
        {
            using (DisposableAction<ISession> usedForDbFetches = this.ActionToBePerformedOnSessionUsedForDbFetches)
                return
                    RepositoryImplBase<T>.DoReportAll<ProjT>(
                        RepositoryHelper<T>.GetExecutableCriteria(usedForDbFetches.Value, criteria,
                                                                  new RepositoryHelper<T>.CriteriaCreator(
                                                                      this.CreateCriteria), new Order[0]),
                        projectionList);
        }

        public ICollection<ProjT> ReportAll<ProjT>(DetachedCriteria criteria, ProjectionList projectionList,
                                                   params Order[] orders)
        {
            using (DisposableAction<ISession> usedForDbFetches = this.ActionToBePerformedOnSessionUsedForDbFetches)
                return
                    RepositoryImplBase<T>.DoReportAll<ProjT>(
                        RepositoryHelper<T>.GetExecutableCriteria(usedForDbFetches.Value, criteria,
                                                                  new RepositoryHelper<T>.CriteriaCreator(
                                                                      this.CreateCriteria), orders), projectionList);
        }

        public ICollection<ProjT> ReportAll<ProjT>(ProjectionList projectionList, params ICriterion[] criteria)
        {
            using (DisposableAction<ISession> usedForDbFetches = this.ActionToBePerformedOnSessionUsedForDbFetches)
                return
                    RepositoryImplBase<T>.DoReportAll<ProjT>(
                        RepositoryHelper<T>.CreateCriteriaFromArray(usedForDbFetches.Value, criteria,
                                                                    new RepositoryHelper<T>.CriteriaCreator(
                                                                        this.CreateCriteria), new Order[0]),
                        projectionList);
        }

        public ICollection<ProjT> ReportAll<ProjT>(ProjectionList projectionList, Order[] orders,
                                                   params ICriterion[] criteria)
        {
            using (DisposableAction<ISession> usedForDbFetches = this.ActionToBePerformedOnSessionUsedForDbFetches)
                return
                    RepositoryImplBase<T>.DoReportAll<ProjT>(
                        RepositoryHelper<T>.CreateCriteriaFromArray(usedForDbFetches.Value, criteria,
                                                                    new RepositoryHelper<T>.CriteriaCreator(
                                                                        this.CreateCriteria), orders), projectionList);
        }

        public ICollection<ProjJ> ReportAll<ProjJ>(string namedQuery, params Parameter[] parameters)
        {
            using (DisposableAction<ISession> usedForDbFetches = this.ActionToBePerformedOnSessionUsedForDbFetches)
                return
                    (ICollection<ProjJ>)
                    RepositoryHelper<T>.CreateQuery(usedForDbFetches.Value, namedQuery, parameters).List<ProjJ>();
        }

        public long Count(DetachedCriteria criteria)
        {
            using (DisposableAction<ISession> usedForDbFetches = this.ActionToBePerformedOnSessionUsedForDbFetches)
            {
                ICriteria executableCriteria = RepositoryHelper<T>.GetExecutableCriteria(usedForDbFetches.Value,
                                                                                         criteria,
                                                                                         new RepositoryHelper<T>.
                                                                                             CriteriaCreator(
                                                                                             this.CreateCriteria),
                                                                                         new Order[0]);
                executableCriteria.SetProjection(new IProjection[1]
                                                     {
                                                         Projections.RowCount()
                                                     });
                return Convert.ToInt64(executableCriteria.UniqueResult());
            }
        }

        public long Count()
        {
            return this.Count((DetachedCriteria) null);
        }

        public bool Exists(DetachedCriteria criteria)
        {
            return 0L != this.Count(criteria);
        }

        public bool Exists()
        {
            return this.Exists((DetachedCriteria) null);
        }

        static ICollection<ProjT> DoReportAll<ProjT>(ICriteria criteria, ProjectionList projectionList)
        {
            return RepositoryImplBase<T>.DoReportAll<ProjT>(criteria, projectionList, false);
        }

        static ICollection<ProjT> DoReportAll<ProjT>(ICriteria criteria, ProjectionList projectionList,
                                                     bool distinctResults)
        {
            RepositoryImplBase<T>.BuildProjectionCriteria<ProjT>(criteria, (IProjection) projectionList, distinctResults);
            return (ICollection<ProjT>) criteria.List<ProjT>();
        }

        static void BuildProjectionCriteria<ProjT>(ICriteria criteria, IProjection projectionList, bool distinctResults)
        {
            if (distinctResults)
                criteria.SetProjection(new IProjection[1]
                                           {
                                               Projections.Distinct(projectionList)
                                           });
            else
                criteria.SetProjection(new IProjection[1]
                                           {
                                               projectionList
                                           });
            if (typeof (ProjT) == typeof (object[]))
                return;
            criteria.SetResultTransformer((IResultTransformer) new RepositoryImplBase<T>.TypedResultTransformer<ProjT>());
        }

        public object ExecuteStoredProcedure(string sp_name, params Parameter[] parameters)
        {
            IConnectionProvider connectionProvider =
                ((ISessionFactoryImplementor) this.SessionFactory).get_ConnectionProvider();
            IDbConnection connection = connectionProvider.GetConnection();
            try
            {
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sp_name;
                    command.CommandType = CommandType.StoredProcedure;
                    RepositoryHelper<T>.CreateDbDataParameters(command, parameters);
                    return command.ExecuteScalar();
                }
            }
            finally
            {
                connectionProvider.CloseConnection(connection);
            }
        }

        public ICollection<T2> ExecuteStoredProcedure<T2>(Converter<IDataReader, T2> converter, string sp_name,
                                                          params Parameter[] parameters)
        {
            IConnectionProvider connectionProvider =
                ((ISessionFactoryImplementor) this.SessionFactory).get_ConnectionProvider();
            IDbConnection connection = connectionProvider.GetConnection();
            try
            {
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sp_name;
                    command.CommandType = CommandType.StoredProcedure;
                    RepositoryHelper<T>.CreateDbDataParameters(command, parameters);
                    IDataReader input = command.ExecuteReader();
                    ICollection<T2> collection = (ICollection<T2>) new List<T2>();
                    while (input.Read())
                        collection.Add(converter(input));
                    input.Close();
                    return collection;
                }
            }
            finally
            {
                connectionProvider.CloseConnection(connection);
            }
        }

        public T Create()
        {
            return (T) Activator.CreateInstance(this.ConcreteType);
        }

        ICriteria CreateCriteria(ISession session)
        {
            return this.CreateDetachedCriteria().GetExecutableCriteria(session);
        }

        class TypedResultTransformer<T1> : IResultTransformer
        {
            public object TransformTuple(object[] tuple, string[] aliases)
            {
                if (tuple.Length == 1)
                    return tuple[0];
                else
                    return Activator.CreateInstance(typeof (T1), tuple);
            }

            IList IResultTransformer.TransformList(IList collection)
            {
                return collection;
            }
        }
    }

    internal class RepositoryHelper<T>
    {
        public static void AddCaching(IQuery query)
        {
            if (!With.Caching.ShouldForceCacheRefresh && With.Caching.Enabled)
            {
                query.SetCacheable(true);
                if (With.Caching.CurrentCacheRegion == null)
                    return;
                query.SetCacheRegion(With.Caching.CurrentCacheRegion);
            }
            else
            {
                if (!With.Caching.ShouldForceCacheRefresh)
                    return;
                query.SetCacheMode((CacheMode)5);
            }
        }

        internal static IQuery CreateQuery(ISession session, string namedQuery, Parameter[] parameters)
        {
            IQuery namedQuery1 = session.GetNamedQuery(namedQuery);
            foreach (Parameter parameter in parameters)
            {
                if (parameter.Type == null)
                    namedQuery1.SetParameter(parameter.Name, parameter.Value);
                else
                    namedQuery1.SetParameter(parameter.Name, parameter.Value, parameter.Type);
            }
            RepositoryHelper<T>.AddCaching(namedQuery1);
            return namedQuery1;
        }

        public static ICriteria GetExecutableCriteria(ISession session, DetachedCriteria criteria, params Order[] orders)
        {
            return RepositoryHelper<T>.GetExecutableCriteria(session, criteria, (RepositoryHelper<T>.CriteriaCreator)delegate
            {
                return session.CreateCriteria(typeof(T));
            }, orders);
        }

        public static ICriteria GetExecutableCriteria(ISession session, DetachedCriteria criteria, RepositoryHelper<T>.CriteriaCreator creator, params Order[] orders)
        {
            ICriteria crit = RepositoryHelper<T>.ApplyFetchingStrategies(criteria == null ? creator(session) : criteria.GetExecutableCriteria(session));
            RepositoryHelper<T>.AddCaching(crit);
            if (orders != null && orders.Length > 0)
            {
                foreach (Order order in orders)
                    crit.AddOrder(order);
            }
            return crit;
        }

        private static ICriteria ApplyFetchingStrategies(ICriteria executableCriteria)
        {
            foreach (IFetchingStrategy fetchingStrategy in IoC.ResolveAll<IFetchingStrategy<T>>())
                executableCriteria = fetchingStrategy.Apply(executableCriteria);
            return executableCriteria;
        }

        public static void AddCaching(ICriteria crit)
        {
            if (With.Caching.ShouldForceCacheRefresh || !With.Caching.Enabled)
                return;
            crit.SetCacheable(true);
            if (With.Caching.CurrentCacheRegion != null)
                crit.SetCacheRegion(With.Caching.CurrentCacheRegion);
        }

        public static ICriteria CreateCriteriaFromArray(ISession session, ICriterion[] criteria, params Order[] orders)
        {
            return RepositoryHelper<T>.CreateCriteriaFromArray(session, criteria, (RepositoryHelper<T>.CriteriaCreator)delegate
            {
                return session.CreateCriteria(typeof(T));
            }, orders);
        }

        public static ICriteria CreateCriteriaFromArray(ISession session, ICriterion[] criteria, RepositoryHelper<T>.CriteriaCreator creator, params Order[] orders)
        {
            ICriteria criteria1 = session.CreateCriteria(typeof(T));
            foreach (ICriterion icriterion in criteria)
            {
                if (icriterion != null)
                    criteria1.Add(icriterion);
            }
            ICriteria crit = RepositoryHelper<T>.ApplyFetchingStrategies(criteria1);
            RepositoryHelper<T>.AddCaching(crit);
            if (orders != null)
            {
                foreach (Order order in orders)
                    crit.AddOrder(order);
            }
            return crit;
        }

        public static void CreateDbDataParameters(IDbCommand command, Parameter[] parameters)
        {
            foreach (Parameter parameter1 in parameters)
            {
                IDbDataParameter parameter2 = command.CreateParameter();
                parameter2.ParameterName = parameter1.Name;
                parameter2.Value = parameter1.Value;
                command.Parameters.Add((object)parameter2);
            }
        }

        public delegate ICriteria CriteriaCreator<T>(ISession session);
    }

}

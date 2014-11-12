namespace Phundus.Core.Specs.Contexts.InMemoryNHibernate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using NHibernate;
    using NHibernate.Criterion;
    using NHibernate.Criterion.Lambda;
    using NHibernate.SqlCommand;
    using NHibernate.Transform;
    using Expression = System.Linq.Expressions.Expression;

    public class InMemoryQueryOver<TRoot, TSubType> : IQueryOver<TRoot, TSubType>
    {
        private IEnumerable<TRoot> _entities;

        public InMemoryQueryOver(IEnumerable<TRoot> entities)
        {
            _entities = entities;
            UnderlyingCriteria = new InMemoryCriteria(OrderByMagic);
        }

        public ICriteria UnderlyingCriteria { get; private set; }
        public ICriteria RootCriteria { get; private set; }

        public IList<TRoot> List()
        {
            return _entities.ToList();
        }

        public IList<U> List<U>()
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, TRoot> ToRowCountQuery()
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, TRoot> ToRowCountInt64Query()
        {
            throw new NotImplementedException();
        }

        public int RowCount()
        {
            throw new NotImplementedException();
        }

        public long RowCountInt64()
        {
            throw new NotImplementedException();
        }

        public TRoot SingleOrDefault()
        {
            return _entities.SingleOrDefault();
        }

        public U SingleOrDefault<U>()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TRoot> Future()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<U> Future<U>()
        {
            throw new NotImplementedException();
        }

        public IFutureValue<TRoot> FutureValue()
        {
            throw new NotImplementedException();
        }

        public IFutureValue<U> FutureValue<U>()
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, TRoot> Clone()
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot> ClearOrders()
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot> Skip(int firstResult)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot> Take(int maxResults)
        {
            return new InMemoryQueryOver<TRoot, TSubType>(_entities.Take(maxResults));
        }

        public IQueryOver<TRoot> Cacheable()
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot> CacheMode(CacheMode cacheMode)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot> CacheRegion(string cacheRegion)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot> ReadOnly()
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, TSubType> And(Expression<Func<TSubType, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, TSubType> And(Expression<Func<bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, TSubType> And(ICriterion expression)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, TSubType> AndNot(Expression<Func<TSubType, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, TSubType> AndNot(Expression<Func<bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryOverRestrictionBuilder<TRoot, TSubType> AndRestrictionOn(
            Expression<Func<TSubType, object>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryOverRestrictionBuilder<TRoot, TSubType> AndRestrictionOn(Expression<Func<object>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, TSubType> Where(Expression<Func<TSubType, bool>> expression)
        {
            // TODO: TRoot und TSubType nicht verstanden...
            var enumerable = _entities.Cast<TSubType>().Where(expression.Compile()).Cast<TRoot>();

            return new InMemoryQueryOver<TRoot, TSubType>(enumerable);
        }

        public IQueryOver<TRoot, TSubType> Where(Expression<Func<bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, TSubType> Where(ICriterion expression)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, TSubType> WhereNot(Expression<Func<TSubType, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, TSubType> WhereNot(Expression<Func<bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryOverRestrictionBuilder<TRoot, TSubType> WhereRestrictionOn(
            Expression<Func<TSubType, object>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryOverRestrictionBuilder<TRoot, TSubType> WhereRestrictionOn(Expression<Func<object>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, TSubType> Select(params Expression<Func<TRoot, object>>[] projections)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, TSubType> Select(params IProjection[] projections)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, TSubType> SelectList(
            Func<QueryOverProjectionBuilder<TRoot>, QueryOverProjectionBuilder<TRoot>> list)
        {
            throw new NotImplementedException();
        }

        public IQueryOverOrderBuilder<TRoot, TSubType> OrderBy(Expression<Func<TSubType, object>> path)
        {
            return new InMemoryQueryOverOrderBuilder<TRoot, TSubType>(this, path);
        }

        public IQueryOverOrderBuilder<TRoot, TSubType> OrderBy(Expression<Func<object>> path)
        {
            throw new NotImplementedException();
        }

        public IQueryOverOrderBuilder<TRoot, TSubType> OrderBy(IProjection projection)
        {
            throw new NotImplementedException();
        }

        public IQueryOverOrderBuilder<TRoot, TSubType> OrderByAlias(Expression<Func<object>> path)
        {
            throw new NotImplementedException();
        }

        public IQueryOverOrderBuilder<TRoot, TSubType> ThenBy(Expression<Func<TSubType, object>> path)
        {
            throw new NotImplementedException();
        }

        public IQueryOverOrderBuilder<TRoot, TSubType> ThenBy(Expression<Func<object>> path)
        {
            throw new NotImplementedException();
        }

        public IQueryOverOrderBuilder<TRoot, TSubType> ThenBy(IProjection projection)
        {
            throw new NotImplementedException();
        }

        public IQueryOverOrderBuilder<TRoot, TSubType> ThenByAlias(Expression<Func<object>> path)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, TSubType> TransformUsing(IResultTransformer resultTransformer)
        {
            throw new NotImplementedException();
        }

        public IQueryOverFetchBuilder<TRoot, TSubType> Fetch(Expression<Func<TRoot, object>> path)
        {
            throw new NotImplementedException();
        }

        public IQueryOverLockBuilder<TRoot, TSubType> Lock()
        {
            throw new NotImplementedException();
        }

        public IQueryOverLockBuilder<TRoot, TSubType> Lock(Expression<Func<object>> alias)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<TSubType, U>> path)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<U>> path)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<TSubType, U>> path, Expression<Func<U>> alias)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<U>> path, Expression<Func<U>> alias)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<TSubType, U>> path, JoinType joinType)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<U>> path, JoinType joinType)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<TSubType, U>> path, Expression<Func<U>> alias,
            JoinType joinType)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<TSubType, U>> path, Expression<Func<U>> alias,
            JoinType joinType, ICriterion withClause)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<U>> path, Expression<Func<U>> alias,
            JoinType joinType)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<U>> path, Expression<Func<U>> alias,
            JoinType joinType, ICriterion withClause)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<TSubType, IEnumerable<U>>> path)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<IEnumerable<U>>> path)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<TSubType, IEnumerable<U>>> path,
            Expression<Func<U>> alias)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<IEnumerable<U>>> path, Expression<Func<U>> alias)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<TSubType, IEnumerable<U>>> path, JoinType joinType)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<IEnumerable<U>>> path, JoinType joinType)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<TSubType, IEnumerable<U>>> path,
            Expression<Func<U>> alias, JoinType joinType)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<TSubType, IEnumerable<U>>> path,
            Expression<Func<U>> alias, JoinType joinType, ICriterion withClause)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<IEnumerable<U>>> path, Expression<Func<U>> alias,
            JoinType joinType)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<IEnumerable<U>>> path, Expression<Func<U>> alias,
            JoinType joinType, ICriterion withClause)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, TSubType> JoinAlias(Expression<Func<TSubType, object>> path,
            Expression<Func<object>> alias)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, TSubType> JoinAlias(Expression<Func<object>> path, Expression<Func<object>> alias)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, TSubType> JoinAlias(Expression<Func<TSubType, object>> path,
            Expression<Func<object>> alias, JoinType joinType)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, TSubType> JoinAlias<U>(Expression<Func<TSubType, U>> path, Expression<Func<U>> alias,
            JoinType joinType, ICriterion withClause)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, TSubType> JoinAlias<U>(Expression<Func<TSubType, IEnumerable<U>>> path,
            Expression<Func<U>> alias, JoinType joinType, ICriterion withClause)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, TSubType> JoinAlias(Expression<Func<object>> path, Expression<Func<object>> alias,
            JoinType joinType)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, TSubType> JoinAlias<U>(Expression<Func<U>> path, Expression<Func<U>> alias,
            JoinType joinType, ICriterion withClause)
        {
            throw new NotImplementedException();
        }

        public IQueryOver<TRoot, TSubType> JoinAlias<U>(Expression<Func<IEnumerable<U>>> path, Expression<Func<U>> alias,
            JoinType joinType, ICriterion withClause)
        {
            throw new NotImplementedException();
        }

        public IQueryOverSubqueryBuilder<TRoot, TSubType> WithSubquery { get; private set; }
        public IQueryOverJoinBuilder<TRoot, TSubType> Inner { get; private set; }
        public IQueryOverJoinBuilder<TRoot, TSubType> Left { get; private set; }
        public IQueryOverJoinBuilder<TRoot, TSubType> Right { get; private set; }
        public IQueryOverJoinBuilder<TRoot, TSubType> Full { get; private set; }

        /// <summary>
        /// http://www.codeproject.com/Articles/235860/Expression-Tree-Basics
        /// </summary>
        /// <param name="order"></param>
        private void OrderByMagic(Order order)
        {
            var orderMethodName = "OrderBy";
            if (order.ToString().Split(' ')[1] == "desc")
                orderMethodName = "OrderByDescending";

            var sortByProp = order.ToString().Split(' ')[0];
            var sortByPropType = typeof (TRoot).GetProperty(sortByProp).PropertyType;

            var pe = Expression.Parameter(typeof (TRoot), "p");
            var expr = Expression.Lambda(Expression.Property(pe, sortByProp), pe);

            MethodInfo orderByMethodInfo = typeof (Queryable)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Single(mi => mi.Name == orderMethodName
                              && mi.IsGenericMethodDefinition
                              && mi.GetGenericArguments().Length == 2
                              && mi.GetParameters().Length == 2
                );

            IQueryable<TRoot> query = _entities.AsQueryable();

            List<TRoot> sortedList = (orderByMethodInfo.MakeGenericMethod(new[] {typeof (TRoot), sortByPropType})
                .Invoke(query, new object[] {query, expr}) as IOrderedQueryable<TRoot>).ToList();

            _entities = sortedList;
        }
    }
}
namespace Phundus.Core.Specs.Contexts.InMemoryNHibernate
{
    using System;
    using System.Linq.Expressions;
    using NHibernate;
    using NHibernate.Criterion.Lambda;
    using NHibernate.Impl;

    public class InMemoryQueryOverOrderBuilder<TRoot, TSubType> : IQueryOverOrderBuilder<TRoot, TSubType>
    {
        public InMemoryQueryOverOrderBuilder(IQueryOver<TRoot, TSubType> root, Expression<Func<TSubType, object>> path) : base(root, path)
        {
            
        }

        public InMemoryQueryOverOrderBuilder(IQueryOver<TRoot, TSubType> root, Expression<Func<object>> path, bool isAlias) : base(root, path, isAlias)
        {
            throw new NotImplementedException();
        }

        public InMemoryQueryOverOrderBuilder(IQueryOver<TRoot, TSubType> root, ExpressionProcessor.ProjectionInfo projection) : base(root, projection)
        {
            throw new NotImplementedException();
        }
    }
}
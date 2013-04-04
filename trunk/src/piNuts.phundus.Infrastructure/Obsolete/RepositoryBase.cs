namespace piNuts.phundus.Infrastructure.Obsolete
{
    using System;
    using System.Collections;
    using NHibernate;
    using NHibernate.Criterion;

    public class RepositoryBase<T> : IRepository<T>
    {
        private Type _concreteType = null;

        public Type ConcreteType
        {
            get
            {
                return this._concreteType ?? typeof(T);
            }
            set
            {
                this._concreteType = value;
            }
        }

        public Func<ISession> SessionFactory { get; set; }

        protected virtual ISession Session
        {
            get { return SessionFactory(); }
        }

        public T Get(object id)
        {
            return (T)this.Session.Get(this.ConcreteType, id);
        }

        public T Load(object id)
        {
            return (T)this.Session.Load(this.ConcreteType, id);
        }

        public void Delete(T entity)
        {
            this.Session.Delete((object)entity);
        }

        public void DeleteAll()
        {
            this.Session.Delete(string.Format("from {0}", (object)this.ConcreteType.Name));
        }

        public void DeleteAll(DetachedCriteria where)
        {
            foreach (object obj in (IEnumerable)where.GetExecutableCriteria(this.Session).List())
                this.Session.Delete(obj);
        }

        public T Save(T entity)
        {
            this.Session.Save((object)entity);
            return entity;
        }

        public T SaveOrUpdate(T entity)
        {
            this.Session.SaveOrUpdate((object)entity);
            return entity;
        }

        public T SaveOrUpdateCopy(T entity)
        {
#pragma warning disable 612,618
            return (T)this.Session.SaveOrUpdateCopy((object)entity);
#pragma warning restore 612,618
        }

        public void Update(T entity)
        {
            this.Session.Update((object)entity);
        }
    }
}
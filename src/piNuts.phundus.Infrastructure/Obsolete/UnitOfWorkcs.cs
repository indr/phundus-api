namespace piNuts.phundus.Infrastructure.Obsolete
{
    using System;
    using System.Reflection;
    using NHibernate;
    using NHibernate.Cfg;

    public interface IUnitOfWorkFactory
    {
    }

    public class NHibernateUnitOfWorkFactory
    {
        public NHibernateUnitOfWorkFactory()
        {
            throw new InvalidOperationException();
        }

        public NHibernateUnitOfWorkFactory(string configurationFileName)
        {
            throw new InvalidOperationException();
        }

        public NHibernateUnitOfWorkFactory(Assembly[] assemblies)
        {
            throw new InvalidOperationException();
        }

        public NHibernateUnitOfWorkFactory(Assembly[] assemblies, string configurationFileName)
            : this(configurationFileName)
        {
            throw new InvalidOperationException();
        }

        public void RegisterSessionFactory(ISessionFactory factory)
        {
            throw new InvalidOperationException();
        }

        public void RegisterSessionFactory(Configuration configuration, ISessionFactory factory)
        {
            throw new InvalidOperationException();
        }
    }

    public interface IUnitOfWork : IDisposable
    {
        void TransactionalFlush();
    }

    public class UnitOfWorkImpl : IUnitOfWork
    {
        public void Dispose()
        {
            // Do nothing
        }

        public void TransactionalFlush()
        {
            // Do nothing
        }
    }

    public static class UnitOfWork
    {
        static IUnitOfWork _globalUnitOfWork;

        public static ISession CurrentSession
        {
            get { return GlobalContainer.Resolve<Func<ISession>>()(); }
            internal set { throw new InvalidOperationException(); }
        }

        public static ISession GetCurrentSessionFor(Type typeOfEntity)
        {
            throw new InvalidOperationException();
        }

        public static IUnitOfWork Start()
        {
            if (_globalUnitOfWork != null)
                return _globalUnitOfWork;
            //TODO: Hmm
            //throw new InvalidOperationException();
            return new UnitOfWorkImpl();
        }

        public static bool IsStarted
        {
            get
            {
                throw new InvalidOperationException();
            }
        }

        public static void RegisterGlobalUnitOfWork(IUnitOfWork result)
        {
            _globalUnitOfWork = result;
        }
    }
}
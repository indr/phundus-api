namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System;
    using NHibernate;

    public class ReadModelBase
    {
        public Func<ISession> SessionFactory { get; set; }

        protected virtual ISession Session
        {
            get { return SessionFactory(); }
        }
    }
}
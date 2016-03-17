namespace Phundus.Common.Infrastructure.Persistence
{
    using System;
    using System.Diagnostics;
    using Castle.Transactions;
    using Common.Domain.Model;
    using NHibernate;

    public class ShortIdGeneratorService : IShortIdGeneratorService
    {
        public Func<ISession> SessionFactory { get; set; }

        protected virtual ISession Session
        {
            get { return SessionFactory(); }
        }

        [Transaction]
        public TShortId GetNext<TShortId>() where TShortId : Identity<int>
        {
            return GetNextShortId<TShortId>();
        }

        private TShortId GetNextShortId<TShortId>() where TShortId : Identity<int>
        {
            var result = GetNextShortId(typeof(TShortId).Name);
            var constructor = typeof(TShortId).GetConstructor(new[] { typeof(int) });
            Debug.Assert(constructor != null, "constructor != null");
            return (TShortId)constructor.Invoke(new object[] { result });
        }

        private int GetNextShortId(string sequenceName)
        {
            // // https://blogs.msdn.microsoft.com/sqlcat/2006/04/10/sql-server-sequence-number/
            var result = Session.CreateSQLQuery("EXEC GetNextSeqVal @name=:name")
                .SetString("name", sequenceName)
                .UniqueResult<int>();

            if (result <= 0)
                throw new Exception("Could not get next sequence value for " + sequenceName + ".");
            return result;
        }
    }
}
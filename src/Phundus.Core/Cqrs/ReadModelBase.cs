namespace Phundus.Core.Cqrs
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using NHibernate;
    using Paging;

    public class ReadModelBase
    {
        public Func<ISession> SessionFactory { get; set; }

        protected virtual ISession Session
        {
            get { return SessionFactory(); }
        }

        private IDataReader ExecuteReader(string sql)
        {
            return CreateCommand(sql).ExecuteReader();
        }

        protected T ExecuteScalar<T>(string sql)
        {
            return (T) CreateCommand(sql).ExecuteScalar();
        }

        private IDbCommand CreateCommand(string sql)
        {
            var command = Session.Connection.CreateCommand();
            command.CommandText = sql;
            return command;
        }

        public T Single<T>(string sql, object arg0)
        {
            return Many<T>(sql, arg0).FirstOrDefault();
        }

        public ICollection<T> Many<T>(string sql)
        {
            using (var reader = ExecuteReader(sql))
            {
                return AutoMapper.Mapper.Map<IDataReader, ICollection<T>>(reader);
            }
        }

        public ICollection<T> Many<T>(string sql, object arg0)
        {
            return Many<T>(String.Format(sql, arg0));
        }

        public PagedResult<T> Paged<T>(string sql, PageRequest page)
        {
            if (!sql.StartsWith("select ", StringComparison.InvariantCultureIgnoreCase))
                throw new ArgumentException("Select command must start with the keyword select");

            var total = GetTotal(sql);
            sql = sql + String.Format(" offset {0} rows fetch next {1} rows only", page.Offset, page.Size);

            return new PagedResult<T>(PageResponse.From(page, total), Many<T>(sql));
        }

        private int GetTotal(string sql)
        {
            var orderByIndex = sql.IndexOf(" order by ", StringComparison.InvariantCultureIgnoreCase);
            if (orderByIndex >= 0)
                sql = sql.Remove(orderByIndex);

            var fromIndex = sql.IndexOf(" from ", StringComparison.InvariantCultureIgnoreCase);

            sql = "select count(*) " + sql.Remove(0, fromIndex);

            return ExecuteScalar<int>(sql);
        }
    }
}
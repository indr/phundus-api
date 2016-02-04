namespace Phundus.Cqrs
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using AutoMapper;
    using NHibernate;
    using Paging;

    public abstract class AutoMappingReadModelBase : ReadModelBase
    {
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
                return Mapper.Map<IDataReader, ICollection<T>>(reader);
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

            // TODO: SQL Server 2012 OFFSET and FETCH
            //sql = sql + String.Format(" offset {0} rows fetch next {1} rows only", page.Offset, page.Size);

            return new PagedResult<T>(PageResponse.From(page, total), Many<T>(sql).Skip(page.Offset).Take(page.Size).ToList());
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
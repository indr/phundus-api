namespace Phundus.Cqrs
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using AutoMapper;
    using Common.Projecting;

    public abstract class AutoMappingReadModelBase : ProjectionBase
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
    }
}
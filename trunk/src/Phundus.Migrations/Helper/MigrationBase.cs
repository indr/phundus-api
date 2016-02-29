namespace Phundus.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using FluentMigrator;

    public abstract class DataMigrationBase : MigrationBase
    {
        protected IDbTransaction Transaction { get; set; }

        protected IDbConnection Connection { get; set; }

        public override void Up()
        {
            Execute.WithConnection((conn, tx) =>
            {
                Connection = conn;
                Transaction = tx;
                Migrate();
            });
        }

        protected abstract void Migrate();

        protected IDbCommand CreateCommand(string commandText)
        {
            var command = Connection.CreateCommand();
            command.Transaction = Transaction;
            command.CommandText = commandText;
            return command;
        }

        public override void Down()
        {
        }
    }

    public abstract class MigrationBase : Migration
    {
        protected const string SchemaName = "dbo";

        protected void Reseed(string tableName, int seed)
        {
            if ((ApplicationContext != null) && (ApplicationContext.ToString().Contains("SqlServerCe")))
                Execute.Sql(String.Format(@"ALTER TABLE [{1}] ALTER COLUMN [Id] IDENTITY ({0}, 1)", seed, tableName));
            else
                Execute.Sql(String.Format(@"DBCC CHECKIDENT ('{1}', reseed, {0})", seed, tableName));
        }

        protected DateTime ConvertLocalToUtc(object local)
        {
            var dateTimeLocal = Convert.ToDateTime(local);
            DateTime.SpecifyKind(dateTimeLocal, DateTimeKind.Local);
            return dateTimeLocal.ToUniversalTime();
        }


        protected void ResetAllProcessedNotificationTrackers()
        {
            Delete.FromTable("ProcessedNotificationTracker").AllRows();
        }

        protected void DeleteTracker(string trackerTypeName)
        {
            Execute.Sql(String.Format(@"DELETE FROM [ProcessedNotificationTracker] WHERE [TypeName] LIKE '%{0}'",
                trackerTypeName));
        }

        protected void EmptyTableAndResetTracker(string tableName, string trackerTypeName)
        {
            if (Schema.Table(tableName).Exists())
                Delete.FromTable(tableName).AllRows();
            DeleteTracker(trackerTypeName);
        }

        protected void DeleteTable(string tableName)
        {
            if (Schema.Table(tableName).Exists())
                Delete.Table(tableName);
        }

        protected void DeleteTableAndResetTracker(string tableName, string trackerTypeName)
        {
            DeleteTable(tableName);
            DeleteTracker(trackerTypeName);
        }
    }

    public abstract class HydrationBase : MigrationBase
    {
        private readonly IList<string> _commands = new List<string>();
        private IDbConnection _conn;
        private IDbTransaction _tx;

        protected IList<string> Commands
        {
            get { return _commands; }
        }

        public override void Up()
        {
            Execute.WithConnection(Hydrate);
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }

        private void Hydrate(IDbConnection conn, IDbTransaction tx)
        {
            _conn = conn;
            _tx = tx;
            Hydrate();
        }

        protected abstract void Hydrate();

        protected IDbCommand CreateCommand(string sql)
        {
            var result = _conn.CreateCommand();
            result.Transaction = _tx;
            result.CommandText = sql;
            return result;
        }

        protected void ExecuteCommands()
        {
            ExecuteCommands(_commands);
        }

        protected void ExecuteCommands(IEnumerable<string> sqls)
        {
            using (var cmd = CreateCommand(null))
            {
                foreach (var each in sqls)
                {
                    cmd.CommandText = each;
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
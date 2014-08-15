namespace phiNdus.fundus.DbMigrations
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using FluentMigrator;

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
    }

    public abstract class HydrationBase : MigrationBase
    {
        private IDbConnection _conn;
        private IDbTransaction _tx;

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

        private readonly IList<string> _commands = new List<string>();

        protected IList<string> Commands { get { return _commands; } }

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
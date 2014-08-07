namespace phiNdus.fundus.DbMigrations
{
    using System.Collections.Generic;
    using System.Data;
    using FluentMigrator;

    public abstract class MigrationBase : Migration
    {
        protected const string SchemaName = "dbo";
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
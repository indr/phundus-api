namespace phiNdus.fundus.DbMigrations
{
    using FluentMigrator;

    public abstract class MigrationBase : Migration
    {
        private string _schemaName = "dbo";

        protected string SchemaName
        {
            get { return _schemaName; }
            set { _schemaName = value; }
        }
    }
}
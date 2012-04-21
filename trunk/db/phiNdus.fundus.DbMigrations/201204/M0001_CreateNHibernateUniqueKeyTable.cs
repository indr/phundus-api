using FluentMigrator;

namespace phiNdus.fundus.DbMigrations
{
    [Migration(201204211635)]
    public class M0001_CreateNHibernateUniqueKeyTable : MigrationBase
    {
        private const string TableName = "hibernate_unique_key";

        public override void Up()
        {
            Create.Table(TableName).InSchema(SchemaName)
                .WithColumn("next_hi").AsInt32();

            Execute.Sql(string.Format("insert into [{0}].hibernate_unique_key values ( 1 );", SchemaName));
        }

        public override void Down()
        {
            Delete.Table(TableName).InSchema(SchemaName);
        }
    }
}
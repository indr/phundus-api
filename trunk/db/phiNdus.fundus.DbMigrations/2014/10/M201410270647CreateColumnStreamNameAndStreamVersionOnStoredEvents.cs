namespace phiNdus.fundus.DbMigrations
{
    using FluentMigrator;

    [Migration(201410270647)]
    public class M201410270647CreateColumnStreamNameAndStreamVersionOnStoredEvents : MigrationBase
    {
        public override void Up()
        {
            Delete.FromTable("StoredEvents").AllRows();

            Delete.Column("AggregateId").FromTable("StoredEvents");

            Create.Column("StreamName").OnTable("StoredEvents").AsString(255).Nullable();
            Create.Column("StreamVersion").OnTable("StoredEvents").AsInt64().Nullable();

            Create.UniqueConstraint().OnTable("StoredEvents").Columns(new[] {"StreamName", "StreamVersion"});
        }

        public override void Down()
        {
        }
    }
}
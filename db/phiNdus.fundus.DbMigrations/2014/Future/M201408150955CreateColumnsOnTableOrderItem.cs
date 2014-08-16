namespace phiNdus.fundus.DbMigrations
{
    using System;
    using FluentMigrator;

    //[Migration(201408150955)]
    public class M201408150955CreateColumnsOnTableOrderItem : MigrationBase
    {
        public override void Up()
        {
            Create.Column("UnitPrice").OnTable("OrderItem").AsDecimal().Nullable();
            Create.Column("Text").OnTable("OrderItem").AsString(255).Nullable();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}
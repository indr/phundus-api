namespace phiNdus.fundus.DbMigrations
{
    using System;
    using FluentMigrator;

    [Migration(201512271247)]
    public class M201512271247DeleteColumnArticleOrganizationIdOnCartItem : MigrationBase
    {
        public override void Up()
        {
            Delete.Column("Article_OrganizationId").FromTable("CartItem").InSchema(SchemaName);
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}
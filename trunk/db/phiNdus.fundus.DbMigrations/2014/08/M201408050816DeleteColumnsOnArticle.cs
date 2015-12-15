namespace phiNdus.fundus.DbMigrations
{
    using System;
    using FluentMigrator;

    [Migration(201408050816)]
    public class M201408050816DeleteColumnsOnArticle : MigrationBase
    {
        public override void Up()
        {
            Delete.Column("Type").FromTable("Article");
            Delete.Column("ParentId").FromTable("Article");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}
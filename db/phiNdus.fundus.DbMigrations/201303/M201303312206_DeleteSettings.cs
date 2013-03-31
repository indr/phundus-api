namespace phiNdus.fundus.DbMigrations
{
    using System.Collections.Generic;
    using FluentMigrator;

    //[Migration(201303312206)]
    public class M201303312206_DeleteSettings : MigrationBase
    {
        public override void Up()
        {
            var settings = new List<string>
                               {
                                   "common.admin-email-address",
                                   "common.server-url",
                                   "mail.smtp.from",
                                   "mail.smtp.host",
                                   "mail.smtp.password",
                                   "mail.smtp.user-name"
                               };
            foreach (var each in settings)
                Delete.FromTable("Setting").InSchema(SchemaName).Row(new { Key = each });
        }

        public override void Down()
        {
        }
    }
}
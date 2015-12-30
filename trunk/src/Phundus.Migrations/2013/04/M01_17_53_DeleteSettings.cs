namespace Phundus.Migrations
{
    using System.Collections.Generic;

    [Dated(0, 2013, 4, 1, 17, 53)]
    public class M01_17_53_DeleteSettings : MigrationBase
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
                Delete.FromTable("Setting").InSchema(SchemaName).Row(new {Key = each});
        }

        public override void Down()
        {
        }
    }
}
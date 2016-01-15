using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201601151012)]
    public class M201601151012_Update_TypeNames_in_StoredEvents : MigrationBase
    {
        public override void Up()
        {
            Execute.Sql(@"
UPDATE [StoredEvents] SET [TypeName] = REPLACE([TypeName], 'Core.', '');
UPDATE [StoredEvents] SET [TypeName] = REPLACE([TypeName], 'IdentityAndAccess.', 'IdentityAccess.');
UPDATE [StoredEvents] SET [TypeName] = REPLACE([TypeName], 'Users.Model.UserRegistered', 'Users.Model.UserSignedUp');
");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}

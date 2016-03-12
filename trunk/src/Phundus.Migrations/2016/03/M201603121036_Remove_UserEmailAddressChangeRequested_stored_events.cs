namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201603121036)]
    public class M201603121036_Remove_UserEmailAddressChangeRequested_stored_events : EventMigrationBase
    {
        protected override void Migrate()
        {
            DeleteStoredEvents(@"Phundus.IdentityAccess.Users.Model.UserEmailAddressChangeRequested");
        }
    }
}
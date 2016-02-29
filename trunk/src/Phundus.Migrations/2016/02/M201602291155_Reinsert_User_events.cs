namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201602291157)]
    public class M201602291157_Reinsert_User_events : EventMigrationBase
    {
        protected override void Migrate()
        {
            Reinsert("Phundus.IdentityAccess.Users.Model.UserEmailAddressChanged, Phundus.Core");
            Reinsert("Phundus.IdentityAccess.Model.Users.UserAddressChanged, Phundus.Core");
        }

        public override void Up()
        {
            base.Up();

            EmptyTableAndResetTracker("Es_Dashboard_EventLog", "EventLogProjection");
        }
    }
}
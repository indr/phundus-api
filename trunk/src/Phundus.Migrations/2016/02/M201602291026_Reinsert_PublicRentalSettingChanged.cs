namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201602291026)]
    public class M201602291026_Reinsert_PublicRentalSettingChanged : EventMigrationBase
    {
        private const string TypeName = @"Phundus.IdentityAccess.Model.PublicRentalSettingChanged, Phundus.Core";

        protected override void Migrate()
        {
            Reinsert(TypeName);
        }

        public override void Up()
        {
            base.Up();

            EmptyTableAndResetTracker("Es_Dashboard_EventLog", "EventLogProjection");
        }
    }
}
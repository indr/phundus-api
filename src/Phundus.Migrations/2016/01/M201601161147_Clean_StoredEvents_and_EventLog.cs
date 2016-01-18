namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201601161147)]
    public class M201601161147_Clean_StoredEvents_and_EventLog : MigrationBase
    {
        public override void Up()
        {
            //Delete.FromTable("Rm_EventLog").AllRows();
            //Delete.FromTable("StoredEvents").AllRows();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}
namespace phiNdus.fundus.DbMigrations
{
    using System;
    using FluentMigrator;

    [Migration(201204211924)]
    public class M0013_InsertRolesAndCreateTrigger : MigrationBase
    {
        public override void Up()
        {
            Execute.Sql(String.Format(@"SET IDENTITY_INSERT [Role] ON;"));//, SchemaName));
            Insert.IntoTable("Role").InSchema(SchemaName)
                .Row(new {Id = 1, Version = 1, Name = "User"});
            Insert.IntoTable("Role").InSchema(SchemaName)
                .Row(new {Id = 2, Version = 1, Name = "Chief"});
            Insert.IntoTable("Role").InSchema(SchemaName)
                .Row(new {Id = 3, Version = 1, Name = "Admin"});
            Execute.Sql(String.Format(@"SET IDENTITY_INSERT [Role] OFF;"));//, SchemaName));

//            Execute.Sql(String.Format(@"CREATE TRIGGER [DenyInsertUpdateDeleteRole] ON [{0}].[Role] AFTER INSERT, UPDATE, DELETE
//AS 
//BEGIN
//  SET NOCOUNT ON;
//  
//  RAISERROR (N'You''re not allowed to insert, update or delete roles', 16, 1);
//END", SchemaName));
        }

        public override void Down()
        {
            // TODO: Drop Trigger

            Execute.Sql(String.Format("DELETE FROM [{0}].[Role]", SchemaName));
        }
    }
}

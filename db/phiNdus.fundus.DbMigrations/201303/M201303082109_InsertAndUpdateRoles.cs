using System;
using FluentMigrator;

namespace phiNdus.fundus.DbMigrations._201204
{
    [Migration(201303082109)]
    public class M201303082109_InsertAndUpdateRoles : MigrationBase
    {
        public override void Up()
        {
            Execute.Sql(String.Format(@"EXEC sp_msforeachtable ""ALTER TABLE ? NOCHECK CONSTRAINT all"""));

            Execute.Sql(String.Format(@"DISABLE TRIGGER [{0}].[DenyInsertUpdateDeleteRole] ON [{0}].[Role];", SchemaName));
            Execute.Sql(String.Format(@"SET IDENTITY_INSERT [{0}].[Role] ON", SchemaName));

            Delete.FromTable("Role").InSchema(SchemaName).AllRows();
            Insert.IntoTable("Role").InSchema(SchemaName)
                .Row(new {Id = 1, Version = 1, Name = "User"});
            Insert.IntoTable("Role").InSchema(SchemaName)
                .Row(new {Id = 2, Version = 1, Name = "Chief"});
            Insert.IntoTable("Role").InSchema(SchemaName)
                .Row(new {Id = 3, Version = 1, Name = "Admin"});

            Execute.Sql(String.Format(@"SET IDENTITY_INSERT [{0}].[Role] OFF", SchemaName));
            Execute.Sql(String.Format(@"ENABLE TRIGGER [{0}].[DenyInsertUpdateDeleteRole] ON [{0}].[Role];", SchemaName));

            Update.Table("User").InSchema(SchemaName).Set(new {RoleId = 3}).Where(new {RoleId = 2});

            Execute.Sql(String.Format(@"exec sp_msforeachtable ""ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all"""));
        }

        public override void Down()
        {
            Update.Table("User").InSchema(SchemaName).Set(new {RoleId = 2}).Where(new {RoleId = 3});

            Execute.Sql(String.Format(@"EXEC sp_msforeachtable ""ALTER TABLE ? NOCHECK CONSTRAINT all"""));

            Execute.Sql(String.Format(@"DISABLE TRIGGER [{0}].[DenyInsertUpdateDeleteRole] ON [{0}].[Role];", SchemaName));
            Execute.Sql(String.Format(@"SET IDENTITY_INSERT [{0}].[Role] ON", SchemaName));

            Delete.FromTable("Role").InSchema(SchemaName).AllRows();
            Insert.IntoTable("Role").InSchema(SchemaName)
                .Row(new {Id = 1, Version = 1, Name = "User"});
            Insert.IntoTable("Role").InSchema(SchemaName)
                .Row(new {Id = 2, Version = 1, Name = "Admin"});
            
            Execute.Sql(String.Format(@"SET IDENTITY_INSERT [{0}].[Role] OFF", SchemaName));
            Execute.Sql(String.Format(@"ENABLE TRIGGER [{0}].[DenyInsertUpdateDeleteRole] ON [{0}].[Role];", SchemaName));

            Execute.Sql(String.Format(@"exec sp_msforeachtable ""ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all"""));
        }
    }
}
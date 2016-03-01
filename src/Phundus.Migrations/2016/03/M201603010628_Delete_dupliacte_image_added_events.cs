namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201603010628)]
    public class M201603010628_Delete_dupliacte_image_added_events : EventMigrationBase
    {
        protected override void Migrate()
        {
            DeleteStoredEvent(new Guid("2931c1d1-c1df-4b5e-ae95-62ded96874f1"));
            DeleteStoredEvent(new Guid("98f0b22c-a3b6-4251-8f1f-a354657ccc62"));
            DeleteStoredEvent(new Guid("27c553ab-5e9c-48bb-ba8e-2c4e7880b9f7"));
            DeleteStoredEvent(new Guid("62a1a172-5aa9-421b-9906-98e0622f8f9c"));
        }
    }
}
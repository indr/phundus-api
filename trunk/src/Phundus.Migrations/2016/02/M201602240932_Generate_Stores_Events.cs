namespace Phundus.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using FluentMigrator;

    [Migration(201602240932)]
    public class M201602240932_Generate_Stores_Events : EventMigrationBase
    {
        protected override void Migrate()
        {
            var command = CreateCommand(@"SELECT [StoreId]
      ,[Version]
      ,[CreatedAtUtc]
      ,[ModifiedAtUtc]
      ,[Address]
      ,[Coordinate_Latitude]
      ,[Coordinate_Longitude]
      ,[OpeningHours]
      ,[Owner_OwnerId]
      ,[Owner_Name]
      ,(SELECT 1 FROM [Dm_IdentityAccess_Organization] WHERE [Guid] = [Owner_OwnerId]) AS IsOrganization
      ,(SELECT 1 FROM [Dm_IdentityAccess_User] WHERE [Guid] = [Owner_OwnerId]) AS IsUser
  FROM [Dm_Inventory_Store]");
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    ProcessRecord(reader);
                }
            }
        }

        private void ProcessRecord(IDataReader reader)
        {
            CreateStoreOpened(reader);
            CreateChangedEventes(reader);
        }

        private void CreateStoreOpened(IDataReader reader)
        {
            var storeId = reader.GetGuid(0);
            var createdAtUtc = reader.GetDateTime(2);
            var ownerId = new OwnerId(reader.GetGuid(8));
            var ownerName = reader.GetString(9);
            var ownerType = reader.IsDBNull(10) ? OwnerType.User : OwnerType.Organization;
            InsertStoredEvent(createdAtUtc, "Phundus.Inventory.Stores.Model.StoreOpened, Phundus.Core", new StoreOpened
            {
                Owner = new Owner
                {
                    Name = ownerName,
                    OwnerId = ownerId,
                    Type = ownerType
                },
                StoreId = storeId
            }, storeId);
        }

        private void CreateChangedEventes(IDataReader reader)
        {
            var storeId = reader.GetGuid(0);
            var modifiedAtUtc = reader.GetDateTime(3);
            if (!reader.IsDBNull(4))
            {
                InsertStoredEvent(modifiedAtUtc, "Phundus.Inventory.Stores.Model.AddressChanged, Phundus.Core", new AddressChanged
                {
                    Address = reader.GetString(4),
                    StoreId = storeId
                }, storeId);
            }
            if (!reader.IsDBNull(5) && !reader.IsDBNull(6))
            {
                InsertStoredEvent(modifiedAtUtc, "Phundus.Inventory.Stores.Model.CoordinateChanged, Phundus.Core", new CoordinateChanged
                {
                    Latitude = Convert.ToDecimal(reader.GetValue(5)),
                    Longitude = Convert.ToDecimal(reader.GetValue(6)),
                    StoreId = storeId
                }, storeId);
            }
            if (!reader.IsDBNull(7))
            {
                InsertStoredEvent(modifiedAtUtc, "Phundus.Inventory.Stores.Model.OpeningHoursChanged, Phundus.Core", new OpeningHoursChanged
                {
                    OpeningHours = reader.GetString(7),
                    StoreId = storeId
                }, storeId);
            }
        }


        [DataContract]
        public class StoreOpened : MigratingDomainEvent
        {
            //[DataMember(Order = 1)]
            //public Object Manager { get; set; }

            [DataMember(Order = 2)]
            public Guid StoreId { get; set; }

            [DataMember(Order = 3)]
            public Owner Owner { get; set; }
        }


        [DataContract]
        public class AddressChanged : MigratingDomainEvent
        {
            //[DataMember(Order = 1)]
            //public Object Manager { get; set; }

            [DataMember(Order = 2)]
            public Guid StoreId { get; set; }

            [DataMember(Order = 3)]
            public string Address { get; set; }
        }

        [DataContract]
        public class CoordinateChanged : MigratingDomainEvent
        {
            //[DataMember(Order = 1)]
            //public Object Manager { get; set; }

            [DataMember(Order = 2)]
            public Guid StoreId { get; set; }

            [DataMember(Order = 3)]
            public decimal Latitude { get; set; }

            [DataMember(Order = 4)]
            public decimal Longitude { get; set; }
        }

        [DataContract]
        public class OpeningHoursChanged : DomainEvent
        {
            //[DataMember(Order = 1)]
            //public Object Manager { get; set; }

            [DataMember(Order = 2)]
            public Guid StoreId { get; set; }

            [DataMember(Order = 3)]
            public string OpeningHours { get; set; }
        }

        [DataContract]
        public class Owner : ValueObject
        {
            private OwnerId _ownerId;
            private string _name;
            private OwnerType _type;

            public virtual OwnerId OwnerId
            {
                get { return _ownerId; }
                set { _ownerId = value; }
            }

            [DataMember(Order = 4)]
            protected virtual Guid OwnerGuid
            {
                get { return OwnerId.Id; }
                set { OwnerId = new OwnerId(value); }
            }

            [DataMember(Order = 2)]
            public virtual OwnerType Type
            {
                get { return _type; }
                set { _type = value; }
            }

            [DataMember(Order = 3)]
            public virtual string Name
            {
                get { return _name; }
                set { _name = value; }
            }

            protected override IEnumerable<object> GetEqualityComponents()
            {
                yield return OwnerId;
            }
        }

        public enum OwnerType
        {
            Unknown,
            Organization,
            User
        }
    }
}
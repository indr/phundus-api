namespace Phundus.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Web.Hosting;
    using CsvHelper;
    using CsvHelper.Configuration;
    using CsvHelper.TypeConversion;
    using FluentMigrator;

    [Profile("Acceptance")]
    public class AcceptanceData : MigrationBase
    {
        public override void Up()
        {
            Delete.FromTable("Dm_Shop_Cart").InSchema(SchemaName).AllRows();
            Delete.FromTable("Dm_Shop_OrderItem").InSchema(SchemaName).AllRows();
            Delete.FromTable("Dm_Shop_Order").InSchema(SchemaName).AllRows();
            Delete.FromTable("Dm_Inventory_ArticleFile").InSchema(SchemaName).AllRows();
            Delete.FromTable("Dm_Inventory_Article").InSchema(SchemaName).AllRows();
            Delete.FromTable("Dm_IdentityAccess_Membership").InSchema(SchemaName).AllRows();
            Delete.FromTable("Rm_Relationships").AllRows();
            Delete.FromTable("Dm_IdentityAccess_Account").InSchema(SchemaName).AllRows();
            Delete.FromTable("Dm_IdentityAccess_User").InSchema(SchemaName).AllRows();
            Delete.FromTable("Dm_IdentityAccess_Organization").InSchema(SchemaName).AllRows();
            Delete.FromTable("Dm_IdentityAccess_Application").InSchema(SchemaName).AllRows();
            Delete.FromTable("Dm_Inventory_Store").InSchema(SchemaName).AllRows();
            Delete.FromTable("StoredEvents").InSchema(SchemaName).AllRows();
            Delete.FromTable("Rm_EventLog").InSchema(SchemaName).AllRows();

            Import<Organization>("Organizations.csv", "Dm_IdentityAccess_Organization", false);
            Import<Store>("Organizations.csv", "Dm_Inventory_Store", false);
            Import<User>("Users.csv", "Dm_IdentityAccess_User");
            Import<Account>("Users.csv", "Dm_IdentityAccess_Account", false);
            Import<Membership>("Memberships.csv", "Dm_IdentityAccess_Membership", false);
            Import<RmRelationship>("Memberships.csv", "Rm_Relationships", false);
            ImportArticle();
            Import<ArticleImage>("ArticleImages.csv", "Dm_Inventory_ArticleFile", false);

            CopyImages();
        }

        private static void CopyImages()
        {
            var sourcePath = HostingEnvironment.MapPath(@"~\App_Data\Seeds\ArticleImages");
            var destinationPath = HostingEnvironment.MapPath(@"~\Content\Images\Articles");

            Directory.Delete(destinationPath, true);

            // http://stackoverflow.com/questions/58744/best-way-to-copy-the-entire-contents-of-a-directory-in-c-sharp#

            // Now Create all of the directories
            foreach (var dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, destinationPath));
            }

            // Copy all the files
            foreach (var newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, destinationPath));
            }
        }

        private void ImportArticle()
        {
            var records = GetRecords<Article>("Articles.csv");
            Execute.Sql(String.Format(@"SET IDENTITY_INSERT [{0}] ON", "Dm_Inventory_Article"));

            var maxId = 0;
            foreach (var each in records)
            {
                maxId = Math.Max(maxId, each.Id);
                Insert.IntoTable("Dm_Inventory_Article").InSchema(SchemaName).Row(new
                    {
                        each.Id,
                        each.Version,
                        each.Owner_OwnerId,
                        each.Owner_Name,
                        CreateDate = DateTime.Now,
                        Name = each.Name,
                        Brand = each.Marke,
                        Price = each.Preis,
                        Description = each.Beschreibung,
                        Stock = each.Bestand,
                        StoreId = each.StoreId
                    });

                }

            Execute.Sql(String.Format(@"SET IDENTITY_INSERT [{0}] OFF", "Dm_Inventory_Article"));

            Reseed("Dm_Inventory_Article", maxId + 1);

            
        }

        private void Import<T>(string fileName, string tableName, bool allowIdentityInsert = true) where T : class
        {
            var records = GetRecords<T>(fileName);

            if (allowIdentityInsert)
                Execute.Sql(String.Format(@"SET IDENTITY_INSERT [{0}] ON", tableName));
            foreach (var each in records)
            {
                Insert.IntoTable(tableName).InSchema(SchemaName).Row(each);
            }
            if (allowIdentityInsert)
                Execute.Sql(String.Format(@"SET IDENTITY_INSERT [{0}] OFF", tableName));
        }

        private static IEnumerable<T> GetRecords<T>(string fileName) where T : class
        {
            var configuration = new CsvConfiguration
                {
                    Delimiter = ";",
                    Encoding = Encoding.UTF8
                };
            var csv = new CsvReader(new StreamReader(HostingEnvironment.MapPath(@"~\App_Data\Seeds\" + fileName)),
                                    configuration);
            return csv.GetRecords<T>();
        }

        public override void Down()
        {
            // Nothing to do here...
        }

        #region Nested type: Article

        internal class Article
        {
            [CsvField(Name = "Id")]
            public int Id { get; set; }

            public int Version
            {
                get { return 1; }
            }

            [CsvField(Name = "Owner_OwnerId")]
            public Guid Owner_OwnerId { get; set; }

            [CsvField(Name = "Owner_Name")]
            public string Owner_Name { get; set; }

            [CsvField(Name = "Name")]
            public string Name { get; set; }

            [CsvField(Name = "Marke")]
            public string Marke { get; set; }

            [CsvField(Name = "Preis")]
            public double Preis { get; set; }

            [CsvField(Name = "Bestand")]
            public double Bestand { get; set; }

            [CsvField(Name = "Beschreibung")]
            public string Beschreibung { get; set; }

            [CsvField(Name = "StoreId")]
            public Guid StoreId { get; set; }
        }

        #endregion

        #region Nested type: ArticleImage

        public class ArticleImage
        {
            private static int _id = 1;

            public int Id
            {
                get { return _id++; }
            }

            public int Version
            {
                get { return 1; }
            }

            [CsvField(Name = "ArtikelId")]
            public int ArticleId { get; set; }

            [CsvField(Name = "Länge")]
            public int Length { get; set; }

            [CsvField(Name = "Typ")]
            public string Type { get; set; }

            [CsvField(Name = "Dateiname")]
            public string FileName { get; set; }
        }

        #endregion

        internal class RmRelationship
        {
            [CsvField(Name = "UserGuid")]
            public Guid UserGuid { get; set; }

            [CsvField(Name = "OrganizationGuid")]
            public Guid OrganizationGuid { get; set; }

            public int Status { get { return 1; } }

            public DateTime Timestamp { get { return DateTime.UtcNow; } }
        }

        #region Nested type: Membership

        internal class Membership
        {
            public int Version
            {
                get { return 1; }
            }

            [CsvField(Name = "UserGuid")]
            public Guid UserGuid { get; set; }

            [CsvField(Name = "OrganizationGuid")]
            public Guid OrganizationGuid { get; set; }

            [CsvField(Name = "Role")]
            [TypeConverter(typeof (RoleConverter))]
            public int Role { get; set; }

            public DateTime ApprovalDate
            {
                get { return DateTime.Now; }
            }

            public bool IsLocked
            {
                get { return false; }
            }
        }

        #endregion

        #region Nested type: Organization

        internal class Organization
        {
            [CsvField(Name = "Guid")]
            public Guid Guid { get; set; }

            public int Version
            {
                get { return 1; }
            }
            
            [CsvField(Name = "Name")]
            public string Name { get; set; }

            [CsvField(Name = "Address")]
            public string Address { get; set; }

            [CsvField(Name = "PhoneNumber")]
            public string PhoneNumber { get; set; }

            [CsvField(Name = "EmailAddress")]
            public string EmailAddress { get; set; }

            [CsvField(Name = "Website")]
            public string Website { get; set; }

            [CsvField(Name = "Plan")]
            public string Plan { get; set; }

            public DateTime CreateDate
            {
                get { return DateTime.UtcNow; }
            }
        }

        internal class Store
        {
            [CsvField(Name = "StoreId")]
            public Guid StoreId { get; set; }

            public int Version
            {
                get { return 1; }
            }

            public DateTime CreatedAtUtc { get { return DateTime.UtcNow; } }
            public DateTime ModifiedAtUtc { get { return DateTime.UtcNow; } }

            [CsvField(Name = "Address")]
            public string Address { get; set; }

            [CsvField(Name = "Guid")]
            public Guid Owner_OwnerId { get; set; }

            [CsvField(Name ="Name")]
            public string Owner_Name { get; set; }
        }

        #endregion
        
        #region Nested type: User

        internal class User
        {
            private string _jsNumber;

            [CsvField(Name = "Id")]
            public int Id { get; set; }

            public int Version
            {
                get { return 1; }
            }

            [CsvField(Name = "UserGuid")]
            public Guid Guid { get; set; }

            [CsvField(Name = "Role")]
            [TypeConverter(typeof (RoleConverter))]
            public int RoleId { get; set; }

            [CsvField(Name = "Vorname")]
            public string FirstName { get; set; }

            [CsvField(Name = "Nachname")]
            public string LastName { get; set; }

            [CsvField(Name = "Strasse")]
            public string Street { get; set; }

            [CsvField(Name = "Plz")]
            public string Postcode { get; set; }

            [CsvField(Name = "Ort")]
            public string City { get; set; }

            [CsvField(Name = "Mobile")]
            public string MobileNumber { get; set; }

            [CsvField(Name = "JS-Nummer")]
            public string JsNumber
            {
                get { return String.IsNullOrWhiteSpace(_jsNumber) ? null : _jsNumber; }
                set { _jsNumber = value; }
            }
        }

        #endregion

        #region Nested type: Account

        internal class Account
        {
            private string _email;

            [CsvField(Name = "Id")]
            public int Id { get; set; }

            public int Version
            {
                get { return 1; }
            }

            public string SessionKey
            {
                get { return null; }
            }

            public string Password
            {
                get { return "d77d9929e85306f64b45956b05c7d767"; }
            }

            public string Salt
            {
                get { return "123ab"; }
            }

            [CsvField(Name = "E-Mail")]
            public string Email
            {
                get
                {
                    if (!String.IsNullOrWhiteSpace(_email))
                        return _email;
                    return Id + "@test.phundus.ch";
                }
                set { _email = value; }
            }

            public int IsApproved
            {
                get { return 1; }
            }

            public int IsLockedOut
            {
                get { return 0; }
            }

            public DateTime CreateDate
            {
                get { return DateTime.Now; }
            }
        }

        #endregion
    }
}
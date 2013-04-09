namespace phiNdus.fundus.DbMigrations
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
            Delete.FromTable("Cart").InSchema(SchemaName).AllRows();
            Delete.FromTable("Order").InSchema(SchemaName).AllRows();
            Delete.FromTable("FieldValue").InSchema(SchemaName).AllRows();
            Delete.FromTable("Image").InSchema(SchemaName).AllRows();
            Delete.FromTable("Article").InSchema(SchemaName).AllRows();
            Delete.FromTable("OrganizationMembership").InSchema(SchemaName).AllRows();
            Delete.FromTable("Membership").InSchema(SchemaName).AllRows();
            Delete.FromTable("User").InSchema(SchemaName).AllRows();
            Delete.FromTable("Organization").InSchema(SchemaName).AllRows();
            Delete.FromTable("Setting").InSchema(SchemaName).AllRows();

            Import<Organization>("Organizations.csv", "Organization");
            Import<User>("Users.csv", "User");
            Import<UserMembership>("Users.csv", "Membership", false);
            Import<Membership>("Memberships.csv", "OrganizationMembership", false);
            ImportArticle();
            Import<Setting>("Settings.csv", "Setting", false);
            Import<ArticleImage>("ArticleImages.csv", "Image", false);

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
            Execute.Sql(String.Format(@"SET IDENTITY_INSERT [{0}].[{1}] ON", SchemaName, "Article"));

            var fieldValueId = 1;
            foreach (var each in records)
            {
                Insert.IntoTable("Article").InSchema(SchemaName).Row(new
                                                                         {
                                                                             each.Id,
                                                                             each.Version,
                                                                             @Type =
                                                                         "phiNdus.fundus.Domain.Entities.Article",
                                                                             each.OrganizationId,
                                                                             CreateDate = DateTime.Now
                                                                         });

                Insert.IntoTable("FieldValue").InSchema(SchemaName).Row(new
                                                                            {
                                                                                Id = fieldValueId++,
                                                                                each.Version,
                                                                                ArticleId = each.Id,
                                                                                FieldDefinitionId = 2,
                                                                                IsDiscriminator = false,
                                                                                TextValue = each.Name
                                                                            });
                Insert.IntoTable("FieldValue").InSchema(SchemaName).Row(new
                                                                            {
                                                                                Id = fieldValueId++,
                                                                                each.Version,
                                                                                ArticleId = each.Id,
                                                                                FieldDefinitionId = 3,
                                                                                IsDiscriminator = false,
                                                                                TextValue = each.Marke
                                                                            });
                Insert.IntoTable("FieldValue").InSchema(SchemaName).Row(new
                                                                            {
                                                                                Id = fieldValueId++,
                                                                                each.Version,
                                                                                ArticleId = each.Id,
                                                                                FieldDefinitionId = 4,
                                                                                IsDiscriminator = false,
                                                                                DecimalValue = each.Preis
                                                                            });
                Insert.IntoTable("FieldValue").InSchema(SchemaName).Row(new
                                                                            {
                                                                                Id = fieldValueId++,
                                                                                each.Version,
                                                                                ArticleId = each.Id,
                                                                                FieldDefinitionId = 8,
                                                                                IsDiscriminator = false,
                                                                                TextValue = each.Beschreibung
                                                                            });
                Insert.IntoTable("FieldValue").InSchema(SchemaName).Row(new
                                                                            {
                                                                                Id = fieldValueId++,
                                                                                each.Version,
                                                                                ArticleId = each.Id,
                                                                                FieldDefinitionId = 10,
                                                                                IsDiscriminator = false,
                                                                                IntegerValue = each.Bestand
                                                                            });
            }

            Execute.Sql(String.Format(@"SET IDENTITY_INSERT [{0}].[{1}] OFF", SchemaName, "Article"));
        }

        private void Import<T>(string fileName, string tableName, bool allowIdentityInsert = true) where T : class
        {
            var records = GetRecords<T>(fileName);

            if (allowIdentityInsert)
                Execute.Sql(String.Format(@"SET IDENTITY_INSERT [{0}].[{1}] ON", SchemaName, tableName));
            foreach (var each in records)
            {
                Insert.IntoTable(tableName).InSchema(SchemaName).Row(each);
            }
            if (allowIdentityInsert)
                Execute.Sql(String.Format(@"SET IDENTITY_INSERT [{0}].[{1}] OFF", SchemaName, tableName));
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

        public class ArticleImage
        {
            private static int _id = 1;
            public int Id { get { return _id++; } }

            public int Version { get { return 1; } }

            [CsvField(Name = "ArtikelId")]
            public int ArticleId { get; set; }

            [CsvField(Name = "Länge")]
            public int Length { get; set; }

            [CsvField(Name = "Typ")]
            public string Type { get; set; }

            [CsvField(Name = "Dateiname")]
            public string FileName { get; set; }
        }

        #region Nested type: Article

        public class Article
        {
            [CsvField(Name = "Id")]
            public int Id { get; set; }

            public int Version
            {
                get { return 1; }
            }

            [CsvField(Name = "OrganizationId")]
            public int OrganizationId { get; set; }

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
        }

        #endregion

        #region Nested type: Membership

        public class Membership
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

            [CsvField(Name = "UserId")]
            public int UserId { get; set; }

            [CsvField(Name = "OrganizationId")]
            public int OrganizationId { get; set; }

            [CsvField(Name = "Role")]
            [TypeConverter(typeof (RoleConverter))]
            public int Role { get; set; }
        }

        #endregion

        #region Nested type: Organization

        internal class Organization
        {
            [CsvField(Name = "Id")]
            public int Id { get; set; }

            public int Version
            {
                get { return 1; }
            }

            [CsvField(Name = "Name")]
            public string Name { get; set; }

            [CsvField(Name = "EmailAddress")]
            public string EmailAddress { get; set; }

            [CsvField(Name = "Website")]
            public string Website { get; set; }
        }

        #endregion

        #region Nested type: Setting

        public class Setting
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

            [CsvField(Name = "Key")]
            public string Key { get; set; }

            [CsvField(Name = "StringValue")]
            public string StringValue { get; set; }
        }

        #endregion

        #region Nested type: User

        public class User
        {
            private string _jsNumber;

            [CsvField(Name = "Id")]
            public int Id { get; set; }

            public int Version
            {
                get { return 1; }
            }

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

        #region Nested type: UserMembership

        public class UserMembership
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
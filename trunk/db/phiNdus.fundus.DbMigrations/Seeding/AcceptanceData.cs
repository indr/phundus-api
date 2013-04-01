using System;
using System.IO;
using System.Text;
using System.Web.Hosting;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using FluentMigrator;

namespace phiNdus.fundus.DbMigrations
{
    [Profile("Acceptance")]
    public class AcceptanceData : MigrationBase
    {
        public override void Up()
        {
            Delete.FromTable("OrganizationMembership").InSchema(SchemaName).AllRows();
            Delete.FromTable("Organization").InSchema(SchemaName).AllRows();
            Delete.FromTable("Membership").InSchema(SchemaName).AllRows();
            Delete.FromTable("User").InSchema(SchemaName).AllRows();


            Import<Organization>("Organizations.csv", "Organization");
            Import<User>("Users.csv", "User");
            Import<UserMembership>("Users.csv", "Membership", false);
            Import<Membership>("Memberships.csv", "OrganizationMembership", false);
        }

        private void Import<T>(string fileName, string tableName, bool allowIdentityInsert = true) where T : class
        {
            var configuration = new CsvConfiguration
                                    {
                                        Delimiter = ";",
                                        Encoding = Encoding.ASCII
                                    };
            var csv = new CsvReader(new StreamReader(HostingEnvironment.MapPath(@"~\App_Data\Seeds\" + fileName)),
                                    configuration);
            var records = csv.GetRecords<T>();

            if (allowIdentityInsert)
                Execute.Sql(String.Format(@"SET IDENTITY_INSERT [{0}].[{1}] ON", SchemaName, tableName));
            foreach (var each in records)
            {
                Insert.IntoTable(tableName).InSchema(SchemaName).Row(each);
            }
            if (allowIdentityInsert)
                Execute.Sql(String.Format(@"SET IDENTITY_INSERT [{0}].[{1}] OFF", SchemaName, tableName));
        }

        public override void Down()
        {
            // Nothing to do here...
        }

        #region Nested type: Organization

        public class Organization
        {
            [CsvField(Name = "Id")]
            public int Id { get; set; }

            public int Version
            {
                get { return 1; }
            }

            [CsvField(Name = "Name")]
            public string Name { get; set; }
        }

        #endregion

        public class Membership
        {
            private static int _id = 1;
            public int Id { get { return _id++; } }

            public int Version
            {
                get { return 1; }
            }

            [CsvField(Name = "UserId")]
            public int UserId { get; set; }

            [CsvField(Name = "OrganizationId")]
            public int OrganizationId { get; set; }

            [CsvField(Name = "Role")]
            [TypeConverter(typeof(RoleConverter))]
            public int Role { get; set; }
        }

        #region Nested type: User

        public class User
        {
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

            private string _jsNumber;

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
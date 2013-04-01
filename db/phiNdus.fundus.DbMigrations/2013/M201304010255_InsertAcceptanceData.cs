using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FluentMigrator;

namespace phiNdus.fundus.DbMigrations
{
    [Tags("Acceptance")]
    [Dated(branch: 0, year: 2013, month: 04, day: 01, hour: 02, minute: 55)]
    public class M201304010255_InsertAcceptanceData : MigrationBase
    {
        private const int User = 1;
        private const int Chief = 2;
        private const int Admin = 3;
        private Regex UserRegex = new Regex("^(?<role>Admin|User), (?<vorname>[^ ]+) (?<nachname>[^,]+), (?<strasse>.+), (?<plz>[0-9]+) (?<ort>.+?)(?:, (?<email>.*))?$");

        private void InsertUsers()
        {
            // http://www.fakenamegenerator.com/gen-random-no-sz.php
            var users = new List<string>
                            {
                                "Admin, Reto Inderbitzin, Sonstwo 42, 6003 Luzern, mail@indr.ch",
                                "User, Davidine Honningsvåg, Wingertweg 96, 2829 Envelier",
                                "User, Ludwik Helgerud, Lichtmattstrasse 69, 4917 Busswil bei Melchnau",
                                "User, Nelborg Abbas, Bahnhofplatz 107, 7473 Alvaneu Bad",
                                "User, Bjørnstjerne Kjelsrud, Höhenweg 132, 3429 Hellsau",
                                "User, Ragnveig Berisha, Clius 118, 5420 Oberehrendingen",
                                "User, Ledis Ensrud, Brunnacherstrasse 55, 8050 Zürich",
                                "User, Nils Oftedal, Via Gabbietta 96, 2829 Vermes",
                                "User, Kay Gleditsch, Sonnenweg 69, 4805 Mättenwil",
                                "User, Salmine Eggum, Seefeldstrasse 3, 3629 Kiesen",
                                "User, Imma Kvam, Via Muraccio 34, 8492 Tablat",
                                "User, Børre Seim, Püntstrasse 121, 3934 Zeneggen",
                                "User, Estella Pollestad, Mattenstrasse 30, 4450 Sissach",
                                "User, Dorte Rødseth, Im Sandbüel 138, 5324 Full",
                                "User, Daghild Høyland, Langwiesstrasse 64, 8372 Wiezikon",
                                "User, Edbjørn Muri, Plattenstrasse 116, 4433 Ramlinsburg",
                                "User, Ernestine Stubberud, Im Wingert 91, 1934 Le Sapey",
                            };

            var id = 1001;
            foreach (var each in users)
                InsertUser(id++, each);
        }

        private void InsertOrganizations()
        {
            Insert.IntoTable("Organization").InSchema(SchemaName)
                .Row(new { Id = 1002, Version = 1, Name = "Pfadi Lego"})
                .Row(new { Id = 1003, Version = 1, Name = "JuBla Playmobil" })
                .Row(new { Id = 1004, Version = 1, Name = "Cevi Dupplo" });
        }

        public override void Up()
        {
            Execute.Sql(String.Format(@"SET IDENTITY_INSERT [{0}].[User] ON", SchemaName));
            InsertUsers();
            Execute.Sql(String.Format(@"SET IDENTITY_INSERT [{0}].[User] OFF", SchemaName));

            Execute.Sql(String.Format(@"SET IDENTITY_INSERT [{0}].[Organization] ON", SchemaName));
            InsertOrganizations();
            Execute.Sql(String.Format(@"SET IDENTITY_INSERT [{0}].[Organization] OFF", SchemaName));
        }

        public override void Down()
        {
            // Nothing to do here...
        }

        private void InsertUser(int id, string address)
        {
            var m = UserRegex.Match(address);
            if (!m.Success)
                throw new Exception("Adresse \"" + address + "\" ist falsch formatiert.");
            var email = m.Groups["email"].Success ? m.Groups["email"].Value : m.Groups["vorname"].Value + '.' + m.Groups["nachname"].Value + "@test.phundus.ch";

            Insert.IntoTable("User").InSchema(SchemaName)
                .Row(new
                         {
                             Id = id,
                             Version = 1, 
                             RoleId = (m.Groups["role"].Value == "Admin") ? 3 : 1,
                             FirstName = m.Groups["vorname"].Value,
                             LastName = m.Groups["nachname"].Value,
                             Street = m.Groups["strasse"].Value,
                             City = m.Groups["ort"].Value,
                             Postcode = m.Groups["plz"].Value
                         });
            Insert.IntoTable("Membership")
                .Row(
                    new
                        {
                            Id = id,
                            Version = 1, 
                            Password = "d77d9929e85306f64b45956b05c7d767",
                            /* 1234 */
                            Salt = "123ab",
                            Email = email,
                            IsApproved = 1,
                            IsLockedOut = 0,
                            CreateDate = DateTime.Now.ToString("yyyy-MM-dd")
                        });
        }
    }
}
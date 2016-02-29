namespace Phundus.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text.RegularExpressions;
    using FluentMigrator;

    [Migration(201602290442)]
    public class M201602290442_Migrate_organization_postal_addresses : DataMigrationBase
    {
        private readonly Regex _phoneNumberRegex = new Regex(@"^((Tel\.:? )|(Telefon ))?(\+?[ 0-9]+)$",
            RegexOptions.Multiline);

        private readonly Regex _postCodeCityRegex = new Regex(@"^(CH-)?([\d]{4}) ([\w ]+)$", RegexOptions.Multiline);

        protected override void Migrate()
        {
            var organizations = GetOrganizations();
            foreach (var each in organizations)
            {
                var address = each.Value.Split(new[] {"\n"}, StringSplitOptions.None).ToList();

                if (address.Count == 0)
                    continue;

                if (address[1] == "")
                    address.RemoveAt(1);
                if (address[2] == "")
                    address.RemoveAt(2);
                address.Remove("Niggi Studer");

                string postcode = null;
                string city = null;
                FindPostcodeAndCity(each.Value, out postcode, out city);

                string phoneNumber = null;
                FindPhoneNumber(each.Value, out phoneNumber);
                if (address.Count >= 4 && address[3] == "")
                {
                    var line1 = address[0];
                    var street = address[1];

                    UpdateAddress(each.Key, line1, null, street, postcode, city, phoneNumber);
                    continue;
                }
                if (address.Count >= 5 && address[4] == "")
                {
                    var line1 = address[0];
                    var line2 = address[1];
                    var street = address[2];

                    UpdateAddress(each.Key, line1, line2, street, postcode, city, phoneNumber);
                    continue;
                }

                throw new Exception(each.Value);
            }
        }

        private void UpdateAddress(Guid organizationId, string line1, string line2, string street, string postcode,
            string city, string phoneNumber)
        {
            var command = CreateCommand(@"
UPDATE [Dm_IdentityAccess_Organization] SET [Line1] = @Line1, [Line2]=@Line2, [Street]=@Street, [Postcode]=@Postcode, [City]=@City, [PhoneNumber]=@PhoneNumber WHERE [Guid]=@OrganizationId");
            
            command.Parameters.Add(new SqlParameter("@Line1", line1));
            if (line2 == null)
                command.Parameters.Add(new SqlParameter("@Line2", DBNull.Value));
            else
                command.Parameters.Add(new SqlParameter("@Line2", line2));

            command.Parameters.Add(new SqlParameter("@Street", street));
            command.Parameters.Add(new SqlParameter("@Postcode", postcode));
            command.Parameters.Add(new SqlParameter("@City", city));
            command.Parameters.Add(new SqlParameter("@PhoneNumber", phoneNumber));
            command.Parameters.Add(new SqlParameter("@OrganizationId", organizationId));
            command.ExecuteNonQuery();
        }

        private void FindPostcodeAndCity(string address, out string postcode, out string city)
        {
            var match = _postCodeCityRegex.Match(address);
            if (!match.Success)
                throw new Exception("Could not find postcode and city in " + address);

            postcode = match.Groups[2].Value;
            city = match.Groups[3].Value;
        }

        private void FindPhoneNumber(string address, out string phoneNumber)
        {
            var match = _phoneNumberRegex.Match(address);
            if (!match.Success)
                throw new Exception("Could not find phone number in " + address);

            phoneNumber = match.Groups[4].Value;
        }


        private IDictionary<Guid, string> GetOrganizations()
        {
            var data = new Dictionary<Guid, string>();

            var command = CreateCommand(@"
SELECT [Guid] As OrganizationId       
      ,[Address]      
  FROM [Dm_IdentityAccess_Organization]");

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (reader.IsDBNull(1))
                        continue;

                    data.Add(reader.GetGuid(0), reader.GetString(1));
                }
            }

            return data;
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}
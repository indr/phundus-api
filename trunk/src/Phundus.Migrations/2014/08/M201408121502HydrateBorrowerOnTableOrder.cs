namespace Phundus.Migrations
{
    using System.Collections.Generic;
    using FluentMigrator;

    [Migration(201408121502)]
    public class M201408121502HydrateBorrowerOnTableOrder : HydrationBase
    {
        protected override void Hydrate()
        {
            var updates = new List<string>();
            const string updateFmt =
                "update [Order] set Borrower_Id = {0}, Borrower_FirstName = '{1}', " +
                "Borrower_LastName = '{2}', Borrower_Street = '{3}', Borrower_Postcode = '{4}', " +
                "Borrower_City = '{5}', Borrower_EmailAddress = '{6}', Borrower_MobilePhoneNumber = '{7}', " +
                "Borrower_MemberNumber = '{8}' where ReserverId = {0}";

            using (var cmd = CreateCommand("select u.*, a.Email from [user] u inner join [membership] a on u.Id = a.Id")
                )
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    updates.Add(
                        string.Format(updateFmt, new[]
                        {
                            reader["Id"],
                            reader["FirstName"].ToString().Replace("'", "''"),
                            reader["LastName"].ToString().Replace("'", "''"),
                            reader["Street"].ToString().Replace("'", "''"),
                            reader["Postcode"].ToString().Replace("'", "''"),
                            reader["City"].ToString().Replace("'", "''"),
                            reader["Email"].ToString().Replace("'", "''"),
                            reader["MobileNumber"].ToString().Replace("'", "''"),
                            reader["JsNumber"].ToString().Replace("'", "''")
                        })
                        );
                }
            }

            ExecuteCommands(updates);
        }
    }
}
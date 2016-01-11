namespace Phundus.Specs.Services.Entities
{
    using System;
    using Assets;
    using TechTalk.SpecFlow;

    /// <summary>
    /// www.fakenamegenerator.com and save it to Phundus.Specs\Resources\FakeNameGenerator.com.csv
    /// </summary>
    [Binding]
    public class FakeNameGenerator : FakeGeneratorBase<FakeNameGeneratorRow>
    {
       public FakeNameGenerator() : base(Resources.FakeNameGenerator_com)
        {
        }

        public Organization NextOrganization()
        {
            var record = GetNextRecord();
            return new Organization
            {
                Guid = new Guid(record.Guid),
                Name = record.Company
            };
        }

        public User NextUser()
        {
            var guid = Guid.NewGuid();
            var record = GetNextRecord();
            var emailAddress = GetEmailAddress(record.EmailAddress, guid);
            return new User
            {
                Password = record.Password,
                FirstName = record.GivenName,
                Guid = guid,
                LastName = record.Surname,
                EmailAddress = emailAddress,
                City = record.City,
                Street = record.StreetAddress,
                MobilePhone = record.TelephoneNumber,
                Postcode = record.ZipCode
            };
        }

        
        private static string GetEmailAddress(string emailAddress, Guid guid)
        {
            var name = emailAddress.Substring(0, emailAddress.IndexOf("@", System.StringComparison.Ordinal));

            return name + "-" + guid.ToString("D").Substring(0, 6) + "@test.phundus.ch";
        }

        
    }
}
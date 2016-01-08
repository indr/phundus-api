namespace Phundus.Specs.Services.Entities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using TechTalk.SpecFlow;

    [Binding]
    public class FakeGenerator
    {
        private int _nextIdx;
        private IList<FakeNameGeneratorRow> _records;
        private int _startIdx = -1;

        public FakeGenerator()
        {
            ReadRecordsFromResource();

            _nextIdx = new Random().Next(0, _records.Count - 1) - 1;            
        }

        private void ReadRecordsFromResource()
        {
            var byteArray = Encoding.UTF8.GetBytes(Resources.Resources.FakeNameGenerator_com);
            var stream = new MemoryStream(byteArray);
            _records = CsvImporter.GetRecords<FakeNameGeneratorRow>(stream);
        }

        public User NextUser()
        {
            var guid = Guid.NewGuid();
            var record = GetNextRecord();
            return new User
            {
                FirstName = record.GivenName,
                Guid = guid,
                LastName = record.Surname,
                EmailAddress = GetEmailAddress(record.EmailAddress, guid),
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

        public Organization NextOrganization()
        {
            var record = GetNextRecord();
            return new Organization
            {
                Guid = new Guid(record.Guid),
                Name = record.Company
            };
        }

        private FakeNameGeneratorRow GetNextRecord()
        {
            if (_startIdx == _nextIdx)
                throw new InvalidOperationException("You have used all the fake name records. Get a bigger file from www.fakenamegenerator.com.");
            if (_startIdx == -1)
                _startIdx = _nextIdx;
            var result = _records[_nextIdx++];

            if (_nextIdx >= _records.Count)
                _nextIdx = 0;

            return result;
        }
    }
}
namespace Phundus.Specs.Services.Entities
{
    using System;
    using System.Collections.Generic;
    using TechTalk.SpecFlow;

    [Binding]
    public class EntityGenerator
    {
        private IList<string> _firstNames = new List<string>
        {
            "Hans",
            "Jakob",
            "Peter",
            "David",
            "Sepp",
            "Karl",
            "Max",
            "Esther",
            "Anna",
            "Franz",
            "Kurt",
            "Patrik",
            "Patrick"
        };

        private IList<string> _lastNames = new List<string>
        {
            "Müller",
            "Amstalden",
            "Bernegger",
            "Dobermann",
            "Escher",
            "Füglistaller",
            "Bucher",
            "Zurmühle",
            "Imbach",
            "Amberg",
            "Imtobel"
        };

        private Random _random;

        public EntityGenerator()
        {
            _random = new System.Random();
        }

        private string RandomTestEmailAddress()
        {
            return String.Format("{0}@test.phundus.ch", Guid.NewGuid().ToString("N").Substring(0, 6));
        }

        private string Random(IList<string> list)
        {
            return list[_random.Next(0, list.Count - 1)];
        }

        public User NextUser()
        {
            var result = new User
            {
                FirstName = Random(_firstNames),
                LastName = Random(_lastNames),
                EmailAddress = RandomTestEmailAddress()
            };
            return result;
        }

        public Organization NextOrganization()
        {
            var result = new Organization
            {
                Name = Random(_lastNames)
            };
            return result;
        }
    }
}
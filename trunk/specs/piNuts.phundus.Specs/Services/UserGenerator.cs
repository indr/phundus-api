namespace Phundus.Specs.Services
{
    using System;
    using System.Collections.Generic;
    using TechTalk.SpecFlow;

    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public Guid Guid { get; set; }
    }

    [Binding]
    public class UserGenerator
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

        public UserGenerator()
        {
            _random = new System.Random();
        }

        public User Generate()
        {
            var result = new User
            {
                FirstName = Random(_firstNames),
                LastName = Random(_lastNames),
                EmailAddress = RandomTestEmailAddress()
            };
            return result;
        }

        private string RandomTestEmailAddress()
        {
            return String.Format("{0}@test.phundus.ch", Guid.NewGuid().ToString("N").Substring(0, 6));
        }

        private string Random(IList<string> list)
        {
            return list[_random.Next(0, list.Count - 1)];
        }

        public User Next()
        {
            return Generate();
        }
    }
}
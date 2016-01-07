namespace Phundus.Specs.Services
{
    using System;
    using Phundus.Rest.Api;
    using TechTalk.SpecFlow;

    [Binding]
    public class RequestContentGenerator
    {
        private readonly UserGenerator _userGenerator;

        public RequestContentGenerator(UserGenerator userGenerator)
        {
            if (userGenerator == null) throw new ArgumentNullException("userGenerator");
            _userGenerator = userGenerator;
        }

        public UsersPostRequestContent GenerateUsersPostRequestContent()
        {
            var randomUser = _userGenerator.Next();
            return new UsersPostRequestContent
            {
                City = "Stadt",
                Email = randomUser.EmailAddress,
                FirstName = randomUser.FirstName,
                LastName = randomUser.LastName,
                MobilePhone = "123",
                Password = "secret",
                Postcode = "6000",
                Street = "Strasse"
            };
        }
    }
}
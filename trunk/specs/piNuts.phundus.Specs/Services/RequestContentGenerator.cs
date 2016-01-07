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

        public UsersPostRequestContent GenerateUsersPostRequestContent(User user)
        {
            return new UsersPostRequestContent
            {
                City = "Stadt",
                Email = user.EmailAddress,
                FirstName = user.FirstName,
                LastName = user.LastName,
                MobilePhone = "123",
                Password = "secret",
                Postcode = "6000",
                Street = "Strasse"
            };
        }
    }
}
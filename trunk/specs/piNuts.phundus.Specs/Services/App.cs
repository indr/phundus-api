namespace Phundus.Specs.Services
{
    using System;
    using System.Net;
    using Api;
    using Phundus.Rest.Api;
    using Phundus.Rest.Api.Admin;
    using Phundus.Rest.ContentObjects;
    using TechTalk.SpecFlow;

    [Binding]
    public class App : AppBase
    {
        private readonly ApiClient _apiClient;
        private readonly RequestContentGenerator _generator;
        private readonly UserGenerator _userGenerator;

        public App(ApiClient apiClient, RequestContentGenerator generator, UserGenerator userGenerator)
        {
            if (apiClient == null) throw new ArgumentNullException("apiClient");
            if (generator == null) throw new ArgumentNullException("generator");
            if (userGenerator == null) throw new ArgumentNullException("userGenerator");
            _apiClient = apiClient;
            _generator = generator;
            _userGenerator = userGenerator;
        }

        public User SignUpUser()
        {
            var user = _userGenerator.Next();
            var response = _apiClient.For<UsersApi>().Post(_generator.GenerateUsersPostRequestContent(user));
            AssertHttpStatus(HttpStatusCode.OK, response);
            user.Guid = response.Data.UserGuid;
            return user;
        }

        public Guid LogIn(string username, string password = "1234")
        {
            var response = _apiClient.For<SessionsApi>()
                .Post(new SessionsPostRequestContent {Username = username, Password = password});
            AssertHttpStatus(HttpStatusCode.OK, response);
            return response.Data.UserGuid;
        }

        public void ConfirmUser(Guid userGuid)
        {
            LogIn("admin@test.phundus.ch");
            var response = _apiClient.For<AdminUsersApi>().Patch(new AdminUsersPatchRequestContent
            {
                IsApproved = true,
                UserGuid = userGuid
            });
            AssertHttpStatus(HttpStatusCode.NoContent, response);
        }

        public void ResetPassword(string emailAddress)
        {
            var response = _apiClient.For<ResetPasswordApi>()
                .Post(new ResetPasswordPostRequestContent { EmailAddress = emailAddress });
            AssertHttpStatus(HttpStatusCode.NoContent, response);
        }
    }
}
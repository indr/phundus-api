namespace Phundus.Specs.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net;
    using ContentTypes;
    using Entities;
    using NUnit.Framework;
    using RestSharp;
    using Steps;
    using TechTalk.SpecFlow;

    [Binding]
    public class App : AppBase
    {
        private readonly ApiClient _apiClient;
        private readonly FakeNameGenerator _fakeNameGenerator;
        private readonly FakeArticleGenerator _fakeArticleGenerator;

        public App(ApiClient apiClient, FakeNameGenerator fakeNameGenerator, FakeArticleGenerator fakeArticleGenerator)
        {
            if (apiClient == null) throw new ArgumentNullException("apiClient");
            if (fakeNameGenerator == null) throw new ArgumentNullException("fakeNameGenerator");
            if (fakeArticleGenerator == null) throw new ArgumentNullException("fakeArticleGenerator");
            _apiClient = apiClient;
            _fakeNameGenerator = fakeNameGenerator;
            _fakeArticleGenerator = fakeArticleGenerator;
        }

        public Response LastResponse { get; private set; }

        [AfterScenario]
        public void DeleteSessionCookies()
        {
            _apiClient.DeleteSessionCookies();
        }

        private void SetLastResponse(IRestResponse restResponse)
        {
            LastResponse = new Response
            {
                StatusCode = restResponse.StatusCode,
                Message = TryGetErrorMessage(restResponse)
            };
        }

        public void LogInAsRoot()
        {
            LogIn("admin@test.phundus.ch");
        }

        public User SignUpUser(string emailAddress = null)
        {
            var user = _fakeNameGenerator.NextUser();
            if (emailAddress != null)
                user.EmailAddress = emailAddress;

            var response = _apiClient.UsersApi
                .Post<UsersPostOkResponseContent>(new UsersPostRequestContent
                {
                    City = user.City,
                    Email = user.EmailAddress,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    MobilePhone = user.MobilePhone,
                    Password = user.Password,
                    Postcode = user.Postcode,
                    Street = user.Street
                });
            AssertHttpStatus(HttpStatusCode.OK, response);
            SetLastResponse(response);
            user.Id = response.Data.UserId;
            user.Guid = response.Data.UserGuid;
            return user;
        }

        public Guid LogIn(string username, string password = "1234", bool assertStatusCode = true)
        {
            var response = _apiClient.SessionsApi
                .Post<SessionsPostOkResponseContent>(new SessionsPostRequestContent
                {
                    Username = username,
                    Password = password
                });
            if (assertStatusCode)
                AssertHttpStatus(HttpStatusCode.OK, response);
            SetLastResponse(response);
            return response.Data.UserGuid;
        }

        public void ConfirmUser(Guid userGuid)
        {
            LogInAsRoot();
            var response = _apiClient.AdminUsersApi
                .Patch(new AdminUsersPatchRequestContent
                {
                    IsApproved = true,
                    UserGuid = userGuid
                });
            DeleteSessionCookies();
            AssertHttpStatus(HttpStatusCode.NoContent, response);
            SetLastResponse(response);
        }

        public void ChangePassword(Guid userGuid, string oldPasswort, string newPassword)
        {
            var response = _apiClient.ChangePasswordApi
                .Post(new ChangePasswordPostRequestContent {OldPassword = oldPasswort, NewPassword = newPassword});
            AssertHttpStatus(HttpStatusCode.NoContent, response);
            SetLastResponse(response);
        }

        public void ResetPassword(string emailAddress)
        {
            var response = _apiClient.ResetPasswordApi
                .Post(new ResetPasswordPostRequestContent {EmailAddress = emailAddress});
            AssertHttpStatus(HttpStatusCode.NoContent, response);
            SetLastResponse(response);
        }

        public bool ChangeEmailAddress(Guid userGuid, string password, string newEmailAddress,
            bool assertStatusCode = true)
        {
            var response = _apiClient.ChangeEmailAddressApi
                .Post(new ChangeEmailAddressPostRequestContent {Password = password, NewEmailAddress = newEmailAddress});
            if (assertStatusCode)
                AssertHttpStatus(HttpStatusCode.NoContent, response);
            SetLastResponse(response);
            return response.StatusCode == HttpStatusCode.NoContent;
        }

        public void SetUsersRole(Guid userGuid, UserRole userRole)
        {
            LogInAsRoot();
            var response = _apiClient.AdminUsersApi
                .Patch(new AdminUsersPatchRequestContent
                {
                    IsAdmin = true,
                    UserGuid = userGuid
                });
            DeleteSessionCookies();
            AssertHttpStatus(HttpStatusCode.NoContent, response);
            SetLastResponse(response);
        }

        public void LockUser(Guid userGuid)
        {
            LogInAsRoot();
            var response = _apiClient.AdminUsersApi
                .Patch(new AdminUsersPatchRequestContent
                {
                    IsLocked = true,
                    UserGuid = userGuid
                });
            DeleteSessionCookies();
            AssertHttpStatus(HttpStatusCode.NoContent, response);
            SetLastResponse(response);
        }

        public void UnlockUser(Guid userGuid)
        {
            LogInAsRoot();
            var response = _apiClient.AdminUsersApi
                .Patch(new AdminUsersPatchRequestContent
                {
                    IsLocked = false,
                    UserGuid = userGuid
                });
            DeleteSessionCookies();
            AssertHttpStatus(HttpStatusCode.NoContent, response);
            SetLastResponse(response);
        }

        public Organization EstablishOrganization(bool assertHttpStatus = true)
        {
            var organization = _fakeNameGenerator.NextOrganization();
            var response = _apiClient.OrganizationsApi
                .Post<OrganizationsPostOkResponseContent>(new OrganizationsPostRequestContent
                {
                    Name = organization.Name
                });
            if (assertHttpStatus)
                AssertHttpStatus(HttpStatusCode.OK, response);
            SetLastResponse(response);
            organization.OrganizationId = response.Data.OrganizationId;
            return organization;
        }

        public IList<Organization> QueryOrganizations()
        {
            var response = _apiClient.OrganizationsApi.Query<Organization>();
            AssertHttpStatus(HttpStatusCode.OK, response);
            SetLastResponse(response);
            return response.Data.Results;
        }

        public void SendFeedback(string senderEmailAddress, string comment)
        {
            var response = _apiClient.FeedbackApi.Post(new FeedbackPostRequestContent
            {
                EmailAddress = senderEmailAddress,
                Comment = comment
            });
            AssertHttpStatus(HttpStatusCode.NoContent, response);
            SetLastResponse(response);
        }

        public void ValidateKey(string validationKey, bool assertStatusCode = true)
        {
            var response = _apiClient.ValidateApi.Post(new {key = validationKey});
            if (assertStatusCode)
                AssertHttpStatus(HttpStatusCode.NoContent, response);
            SetLastResponse(response);
        }

        public Organization GetOrganization(Guid organizationGuid)
        {
            var response = _apiClient.OrganizationsApi
                .Get<Organization>(new { organizationGuid });
            AssertHttpStatus(HttpStatusCode.OK, response);
            SetLastResponse(response);
            return response.Data;
        }

        public void OpenUserStore(User user, bool assertStatusCode = true)
        {
            var response = _apiClient.StoresApi
                .Post<StoresPostOkResponseContent>(new StoresPostRequestContent
                {
                    UserId = user.Id
                });
            if (assertStatusCode)
                AssertHttpStatus(HttpStatusCode.OK, response);
            SetLastResponse(response);
            user.StoreId = response.Data.StoreId;
        }

        public UsersGetOkResponseContent GetUser(int userId)
        {
            var response = _apiClient.UsersApi.Get<UsersGetOkResponseContent>(new {userId = userId});
            AssertHttpStatus(HttpStatusCode.OK, response);
            SetLastResponse(response);
            return response.Data;
        }

        public void CreateArticle(User user)
        {
            var fakeArticle = _fakeArticleGenerator.NextArticle();
            var response = _apiClient.ArticlesApi.Post<ArticlesPostOkResponseContent>(new ArticlesPostRequestContent
            {
                Amount = fakeArticle.GrossStock,
                Name = fakeArticle.Name,
                OwnerId = user.Id.ToString(CultureInfo.InvariantCulture)
            });
            AssertHttpStatus(HttpStatusCode.OK, response);
        }

        public QueryOkResponseContent<Article> QueryArticlesByUser(User user)
        {
            var response = _apiClient.ArticlesApi.Query<Article>(new {ownerId = user.Id});
            AssertHttpStatus(HttpStatusCode.OK, response);
            SetLastResponse(response);
            return response.Data;
        }

        public StatusGetOkResponseContent GetStatus()
        {
            var response = _apiClient.StatusApi.Get<StatusGetOkResponseContent>(null);
            AssertHttpStatus(HttpStatusCode.OK, response);
            return response.Data;
        }
    }

    public class Response
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }

        /// <summary>
        /// True if StatusCode is 2xx, otherwise false.
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                var statusCode = (int) StatusCode;
                return (statusCode >= 200 && statusCode < 300);
            }
        }
    }
}
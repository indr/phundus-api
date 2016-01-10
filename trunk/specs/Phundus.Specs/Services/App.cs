namespace Phundus.Specs.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Api;
    using Entities;
    using NUnit.Framework;
    using Phundus.Rest.Api;
    using Phundus.Rest.Api.Account;
    using Phundus.Rest.Api.Admin;
    using Phundus.Rest.ContentObjects;
    using Steps;
    using TechTalk.SpecFlow;
    using Organization = Entities.Organization;

    [Binding]
    public class App : AppBase
    {
        private readonly ApiClient _apiClient;
        private readonly FakeGenerator _fakeGenerator;

        public App(ApiClient apiClient, FakeGenerator fakeGenerator)
        {
            if (apiClient == null) throw new ArgumentNullException("apiClient");
            if (fakeGenerator == null) throw new ArgumentNullException("fakeGenerator");
            _apiClient = apiClient;
            _fakeGenerator = fakeGenerator;
        }

        public User SignUpUser()
        {
            var user = _fakeGenerator.NextUser();
            var response = _apiClient.For<UsersApi>()
                .Post(new UsersPostRequestContent
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
            user.Guid = response.Data.UserGuid;
            return user;
        }

        public Guid LogIn(string username, string password = "1234", bool assertStatusCode = true)
        {
            var response = _apiClient.For<SessionsApi>()
                .Post(new SessionsPostRequestContent {Username = username, Password = password});
            if (assertStatusCode) 
                AssertHttpStatus(HttpStatusCode.OK, response);
            return response.Data.UserGuid;
        }

        public void ConfirmUser(Guid userGuid)
        {
            var response = _apiClient.For<AdminUsersApi>()
                .Patch(new AdminUsersPatchRequestContent
                {
                    IsApproved = true,
                    UserGuid = userGuid
                });
            AssertHttpStatus(HttpStatusCode.NoContent, response);
        }

        public void LogInAsRoot()
        {
            LogIn("admin@test.phundus.ch");
        }

        public void ChangePassword(Guid userGuid, string oldPasswort, string newPassword)
        {
            var response = _apiClient.For<ChangePasswordApi>()
                .Post(new ChangePasswordPostRequestContent { OldPassword = oldPasswort, NewPassword = newPassword });
            AssertHttpStatus(HttpStatusCode.NoContent, response);
        }

        public void ResetPassword(string emailAddress)
        {
            var response = _apiClient.For<ResetPasswordApi>()
                .Post(new ResetPasswordPostRequestContent {EmailAddress = emailAddress});
            AssertHttpStatus(HttpStatusCode.NoContent, response);
        }

        public bool ChangeEmailAddress(Guid userGuid, string password, string newEmailAddress, bool assertStatusCode = true)
        {
            var response = _apiClient.For<ChangeEmailAddressApi>()
                .Post(new ChangeEMailAddressPostRequestContent { Password = password, NewEmailAddress = newEmailAddress });
            if (assertStatusCode)
                AssertHttpStatus(HttpStatusCode.NoContent, response);
            return response.StatusCode == HttpStatusCode.NoContent;
        }

        public void SetUsersRole(Guid userGuid, UserRole userRole)
        {
            var response = _apiClient.For<AdminUsersApi>()
                .Patch(new AdminUsersPatchRequestContent
                {
                    IsAdmin = true,
                    UserGuid = userGuid
                });
            AssertHttpStatus(HttpStatusCode.NoContent, response);
        }

        public void LockUser(Guid userGuid)
        {
            var response = _apiClient.For<AdminUsersApi>()
                .Patch(new AdminUsersPatchRequestContent
                {
                    IsLocked = true,
                    UserGuid = userGuid
                });
            AssertHttpStatus(HttpStatusCode.NoContent, response);
        }

        public void UnlockUser(Guid userGuid)
        {
            var response = _apiClient.For<AdminUsersApi>()
                .Patch(new AdminUsersPatchRequestContent
                {
                    IsLocked = false,
                    UserGuid = userGuid
                });
            AssertHttpStatus(HttpStatusCode.NoContent, response);
        }

        public Organization EstablishOrganization()
        {
            var organization = _fakeGenerator.NextOrganization();
            var response = _apiClient.For<OrganizationsApi>()
                .Post<OrganizationsPostOkResponseContent>(new OrganizationsPostRequestContent
                {
                    Name = organization.Name
                });
            AssertHttpStatus(HttpStatusCode.OK, response);
            organization.Guid = response.Data.OrganizationId;
            return organization;
        }

        public IList<Phundus.Rest.ContentObjects.Organization> QueryOrganizations()
        {
            var response = _apiClient.For<OrganizationsApi>()
                .Query<OrganizationsQueryOkResponseContent>();
            AssertHttpStatus(HttpStatusCode.OK, response);
            return response.Data.Results;
        }

        public void DeleteSessionCookies()
        {
            _apiClient.DeleteSessionCookies();
        }

        public void SendFeedback(string senderEmailAddress, string comment)
        {
            var response = _apiClient.For<FeedbackApi>().Post(new FeedbackPostRequestContent
            {
                EmailAddress = senderEmailAddress,
                Comment = comment
            });
            AssertHttpStatus(HttpStatusCode.NoContent, response);
        }

        public bool ValidateKey(string validationKey, bool assertStatusCode = true)
        {
            var response = _apiClient.For<ValidateApi>()
                .Post(new {key = validationKey});
            if (assertStatusCode)
                AssertHttpStatus(HttpStatusCode.NoContent, response);
            return response.StatusCode == HttpStatusCode.NoContent;
        }
    }
}
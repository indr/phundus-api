﻿namespace Phundus.Specs.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Entities;
    using Phundus.Rest.Api;
    using Phundus.Rest.Api.Account;
    using Phundus.Rest.Api.Admin;
    using RestSharp;
    using Steps;
    using TechTalk.SpecFlow;

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

        private void LogInAsRoot()
        {
            LogIn("admin@test.phundus.ch");
        }

        public User SignUpUser(string emailAddress = null)
        {
            var user = _fakeGenerator.NextUser();
            if (emailAddress != null)
                user.EmailAddress = emailAddress;

            var response = _apiClient.UsersApi()
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
            var response = _apiClient.SessionsApi()
                .Post<SessionsPostOkResponseContent>(new SessionsPostRequestContent { Username = username, Password = password });
            if (assertStatusCode)
                AssertHttpStatus(HttpStatusCode.OK, response);
            SetLastResponse(response);
            return response.Data.UserGuid;
        }

        public void ConfirmUser(Guid userGuid)
        {
            LogInAsRoot();
            var response = _apiClient.AdminUsersApi()
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
            var response = _apiClient.ChangePasswordApi()
                .Post(new ChangePasswordPostRequestContent {OldPassword = oldPasswort, NewPassword = newPassword});
            AssertHttpStatus(HttpStatusCode.NoContent, response);
            SetLastResponse(response);
        }

        public void ResetPassword(string emailAddress)
        {
            var response = _apiClient.ResetPasswordApi()
                .Post(new ResetPasswordPostRequestContent {EmailAddress = emailAddress});
            AssertHttpStatus(HttpStatusCode.NoContent, response);
            SetLastResponse(response);
        }

        public bool ChangeEmailAddress(Guid userGuid, string password, string newEmailAddress,
            bool assertStatusCode = true)
        {
            var response = _apiClient.ChangeEmailAddressApi()
                .Post(new ChangeEMailAddressPostRequestContent {Password = password, NewEmailAddress = newEmailAddress});
            if (assertStatusCode)
                AssertHttpStatus(HttpStatusCode.NoContent, response);
            SetLastResponse(response);
            return response.StatusCode == HttpStatusCode.NoContent;
        }

        public void SetUsersRole(Guid userGuid, UserRole userRole)
        {
            LogInAsRoot();
            var response = _apiClient.AdminUsersApi()
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
            var response = _apiClient.AdminUsersApi()
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
            var response = _apiClient.AdminUsersApi()
                .Patch(new AdminUsersPatchRequestContent
                {
                    IsLocked = false,
                    UserGuid = userGuid
                });
            DeleteSessionCookies();
            AssertHttpStatus(HttpStatusCode.NoContent, response);
            SetLastResponse(response);
        }

        public Organization EstablishOrganization()
        {
            var organization = _fakeGenerator.NextOrganization();
            var response = _apiClient.OrganizationsApi()
                .Post<OrganizationsPostOkResponseContent>(new OrganizationsPostRequestContent
                {
                    Name = organization.Name
                });
            AssertHttpStatus(HttpStatusCode.OK, response);
            SetLastResponse(response);
            organization.Guid = response.Data.OrganizationId;
            return organization;
        }

        public IList<Phundus.Rest.ContentObjects.Organization> QueryOrganizations()
        {
            var response = _apiClient.OrganizationsApi()
                .Query<OrganizationsQueryOkResponseContent>();
            AssertHttpStatus(HttpStatusCode.OK, response);
            SetLastResponse(response);
            return response.Data.Results;
        }

        public void SendFeedback(string senderEmailAddress, string comment)
        {
            var response = _apiClient.FeedbackApi().Post(new FeedbackPostRequestContent
            {
                EmailAddress = senderEmailAddress,
                Comment = comment
            });
            AssertHttpStatus(HttpStatusCode.NoContent, response);
            SetLastResponse(response);
        }

        public void ValidateKey(string validationKey, bool assertStatusCode = true)
        {
            var response = _apiClient.ValidateApi()
                .Post(new {key = validationKey});
            if (assertStatusCode)
                AssertHttpStatus(HttpStatusCode.NoContent, response);
            SetLastResponse(response);
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
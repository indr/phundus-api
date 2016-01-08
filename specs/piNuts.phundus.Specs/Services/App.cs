namespace Phundus.Specs.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Api;
    using Entities;
    using NUnit.Framework;
    using Phundus.Rest.Api;
    using Phundus.Rest.Api.Admin;
    using Phundus.Rest.ContentObjects;
    using RestSharp;
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
                    Password = "secret",
                    Postcode = user.Postcode,
                    Street = user.Street
                });
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

        public void ResetPassword(string emailAddress)
        {
            var response = _apiClient.For<ResetPasswordApi>()
                .Post(new ResetPasswordPostRequestContent {EmailAddress = emailAddress});
            AssertHttpStatus(HttpStatusCode.NoContent, response);
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
    }
}
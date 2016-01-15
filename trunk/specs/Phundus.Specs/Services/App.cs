namespace Phundus.Specs.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using ContentTypes;
    using Entities;
    using TechTalk.SpecFlow;

    [Binding]
    public class App : AppBase
    {
        private readonly ApiClient _apiClient;
        private readonly FakeArticleGenerator _fakeArticleGenerator;
        private readonly FakeNameGenerator _fakeNameGenerator;

        public App(ApiClient apiClient, FakeNameGenerator fakeNameGenerator, FakeArticleGenerator fakeArticleGenerator)
        {
            if (apiClient == null) throw new ArgumentNullException("apiClient");
            if (fakeNameGenerator == null) throw new ArgumentNullException("fakeNameGenerator");
            if (fakeArticleGenerator == null) throw new ArgumentNullException("fakeArticleGenerator");
            _apiClient = apiClient;
            _fakeNameGenerator = fakeNameGenerator;
            _fakeArticleGenerator = fakeArticleGenerator;
        }

        [AfterScenario]
        public void DeleteSessionCookies()
        {
            _apiClient.DeleteSessionCookies();
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

            user.Id = response.Data.UserId;
            user.Guid = response.Data.UserGuid;
            return user;
        }

        public Guid LogIn(string username, string password = "1234", bool assertStatusCode = true)
        {
            var response = _apiClient.Assert(assertStatusCode).SessionsApi
                .Post<SessionsPostOkResponseContent>(new SessionsPostRequestContent
                {
                    Username = username,
                    Password = password
                });
            return response.Data.UserGuid;
        }

        public void ConfirmUser(Guid userGuid)
        {
            LogInAsRoot();
            _apiClient.AdminUsersApi
                .Patch(new AdminUsersPatchRequestContent
                {
                    IsApproved = true,
                    UserGuid = userGuid
                });
            DeleteSessionCookies();
        }

        public void ChangePassword(Guid userGuid, string oldPasswort, string newPassword)
        {
            _apiClient.ChangePasswordApi
                .Post(new ChangePasswordPostRequestContent {OldPassword = oldPasswort, NewPassword = newPassword});
        }

        public void ResetPassword(string emailAddress)
        {
            _apiClient.ResetPasswordApi
                .Post(new ResetPasswordPostRequestContent {EmailAddress = emailAddress});
        }

        public bool ChangeEmailAddress(Guid userGuid, string password, string newEmailAddress,
            bool assertStatusCode = true)
        {
            var response = _apiClient.Assert(assertStatusCode).ChangeEmailAddressApi
                .Post(new ChangeEmailAddressPostRequestContent {Password = password, NewEmailAddress = newEmailAddress});
            return response.StatusCode == HttpStatusCode.NoContent;
        }

        public void SetUsersRole(Guid userGuid, UserRole userRole)
        {
            LogInAsRoot();
            _apiClient.AdminUsersApi
                .Patch(new AdminUsersPatchRequestContent
                {
                    IsAdmin = true,
                    UserGuid = userGuid
                });
            DeleteSessionCookies();
        }

        public void LockUser(Guid userGuid)
        {
            LogInAsRoot();
            _apiClient.AdminUsersApi
                .Patch(new AdminUsersPatchRequestContent
                {
                    IsLocked = true,
                    UserGuid = userGuid
                });
            DeleteSessionCookies();
        }

        public void UnlockUser(Guid userGuid)
        {
            LogInAsRoot();
            _apiClient.AdminUsersApi
                .Patch(new AdminUsersPatchRequestContent
                {
                    IsLocked = false,
                    UserGuid = userGuid
                });
            DeleteSessionCookies();
        }

        public Organization EstablishOrganization(bool assertHttpStatus = true)
        {
            var organization = _fakeNameGenerator.NextOrganization();
            var response = _apiClient.Assert(assertHttpStatus).OrganizationsApi
                .Post<OrganizationsPostOkResponseContent>(new OrganizationsPostRequestContent
                {
                    Name = organization.Name
                });
            organization.OrganizationId = response.Data.OrganizationId;
            return organization;
        }

        public IList<Organization> QueryOrganizations()
        {
            var response = _apiClient.OrganizationsApi.Query<Organization>();
            return response.Data.Results;
        }

        public void SendFeedback(string senderEmailAddress, string comment)
        {
            _apiClient.FeedbackApi.Post(new FeedbackPostRequestContent
            {
                EmailAddress = senderEmailAddress,
                Comment = comment
            });
        }

        public void ValidateKey(string validationKey, bool assertStatusCode = true)
        {
            _apiClient.Assert(assertStatusCode).ValidateApi.Post(new {key = validationKey});
        }

        public Organization GetOrganization(Guid organizationGuid)
        {
            var response = _apiClient.OrganizationsApi
                .Get<Organization>(new {organizationGuid});
            return response.Data;
        }

        public void OpenUserStore(User user, bool assertStatusCode = true)
        {
            var response = _apiClient.Assert(assertStatusCode).StoresApi
                .Post<StoresPostOkResponseContent>(new StoresPostRequestContent
                {
                    UserId = user.Id
                });
            user.StoreId = response.Data.StoreId;
        }

        public UsersGetOkResponseContent GetUser(int userId)
        {
            var response = _apiClient.UsersApi.Get<UsersGetOkResponseContent>(new {userId});
            return response.Data;
        }

        public Article CreateArticle(Guid ownerGuid, TableRow row = null)
        {
            var article = _fakeArticleGenerator.NextArticle(row);
            article.OwnerId = ownerGuid;
            var response = _apiClient.ArticlesApi.Post<ArticlesPostOkResponseContent>(new ArticlesPostRequestContent
            {
                Amount = article.GrossStock,
                Name = article.Name,
                OwnerGuid = article.OwnerId
            });
            article.ArticleId = response.Data.ArticleId;
            return article;
        }

        public QueryOkResponseContent<Article> QueryArticlesByUser(User user)
        {
            var response = _apiClient.ArticlesApi.Query<Article>(new {ownerId = user.Id});
            return response.Data;
        }

        public StatusGetOkResponseContent GetStatus()
        {
            var response = _apiClient.StatusApi.Get<StatusGetOkResponseContent>(null);
            return response.Data;
        }

        public UsersCartGetOkResponseContent GetCart(User user, bool assertHttpStatus = true)
        {
            var response = _apiClient.Assert(assertHttpStatus).UserCartApi
                .Get<UsersCartGetOkResponseContent>(new {userGuid = user.Guid});
            return response.Data;
        }

        public Guid ApplyForMembership(User user, Organization organization)
        {
            var response = _apiClient.OrganizationsApplicationsApi
                .Post<OrganizationsApplicationsPostOkResponseContent>(new {organizationId = organization.OrganizationId});
            return response.Data.ApplicationId;
        }

        public OrganizationsRelationshipsQueryOkResponseContent GetRelationshipStatus(User user,
            Organization organization)
        {
            var response = _apiClient.OrganizationsRelationshipsApi
                .Get<OrganizationsRelationshipsQueryOkResponseContent>(
                    new {organizationId = organization.OrganizationId});
            return response.Data;
        }

        public void RejectMembershipApplication(Organization organization, Guid applicationId)
        {
            _apiClient.OrganizationsApplicationsApi
                .Delete(new {organizationId = organization.OrganizationId, applicationId});
        }

        public void ApproveMembershipApplication(Organization organization, Guid applicationId)
        {
            _apiClient.OrganizationsMembersApi
                .Post(new {organizationId = organization.OrganizationId, applicationId});
        }

        public IList<Member> GetOrganizationMembers(Organization organization)
        {
            var response = _apiClient.OrganizationsMembersApi
                .Query<Member>(new {organizationId = organization.OrganizationId});
            return response.Data.Results;
        }

        public Guid AddArticleToCart(User user, Article article)
        {
            var response = _apiClient.UserCartItemsApi
                .Post<UserCartItemsPostOkResponseContent>(new
                {
                    userId = user.Id,
                    userGuid = user.Guid,
                    articleId = article.ArticleId,
                    quantity = 1,
                    fromUtc = DateTime.UtcNow,
                    toUtc = DateTime.UtcNow.AddDays(1)
                });
            return response.Data.CartItemId;
        }

        public void RemoveCartItem(User user, Guid cartItemId)
        {
            _apiClient.UserCartItemsApi
                .Delete(new {userGuid = user.Guid, itemId = cartItemId});
        }

        public bool CheckAvailability(Article article, int quantity)
        {
            var result = _apiClient.ShopItemsAvailabilityCheck
                .Post<ShopItemsAvailabilityCheckOkResponseContent>(
                    new
                    {
                        itemId = article.ArticleId,
                        quantity,
                        fromUtc = DateTime.UtcNow,
                        toUtc = DateTime.UtcNow.AddDays(1)
                    });
            return result.Data.IsAvailable;
        }

        public void ChangeMembersRole(Guid organizationId, int memberId, MemberRole role)
        {
            _apiClient.OrganizationsMembersApi.Patch(
                new {organizationId, memberId, isManager = role == MemberRole.Manager});
        }

        public int CreateOrder(Guid organizationId, int lesseeId)
        {
            var result = _apiClient.OrdersApi.Post<OrdersPostOkResponseContent>(new {ownerId = organizationId, lesseeId});
            return result.Data.OrderId;
        }

        public void LogIn(User user)
        {
            LogIn(user.Username, user.Password);
        }

        internal IList<Order> QueryOrders(Guid organizationId)
        {
            return _apiClient.OrdersApi.Query<Order>(new {organizationId}).Data.Results;
        }

        public void AddOrderItem(int orderId, int articleId)
        {
            _apiClient.OrdersItemsApi.Post(new OrdersItemsPostRequestContent
            {
                OrderId = orderId,
                ArticleId = articleId,
                FromUtc = DateTime.UtcNow,
                ToUtc = DateTime.UtcNow.AddDays(1),
                Quantity = 1
            });
        }

        public int PlaceOrder(User user, Guid lessorGuid)
        {
            return _apiClient.ShopOrdersApi
                .Post<ShopOrdersPostOkResponseContent>(new ShopOrdersPostRequestContent
                {
                    LessorGuid = lessorGuid
                })
                .Data.OrderId;
        }

        public void UpdateStartpage(Organization organization, string htmlContent)
        {
            _apiClient.OrganizationsApi.Patch(new {organizationGuid = organization.Guid, startpage = htmlContent});
        }
    }
}
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
        private static bool _inMaintenanceMode;
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

        public static bool InMaintenanceMode
        {
            get { return _inMaintenanceMode; }
            set { _inMaintenanceMode = value; }
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

        public User SignUpUser(string emailAddress = null, Guid? organizationId = null, bool assertHttpStatus = true)
        {
            var user = _fakeNameGenerator.NextUser();
            if (emailAddress != null)
                user.EmailAddress = emailAddress;

            var response = _apiClient.Assert(assertHttpStatus).UsersApi
                .Post<UsersPostOkResponseContent>(new UsersPostRequestContent
                {
                    City = user.City,
                    Email = user.EmailAddress,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    MobilePhone = user.MobilePhone,
                    Password = user.Password,
                    Postcode = user.Postcode,
                    Street = user.Street,
                    OrganizationId = organizationId
                });


            user.UserId = response.Data.UserId;
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
            return response.Data.UserId;
        }

        public void ConfirmUser(Guid userId)
        {
            LogInAsRoot();
            _apiClient.AdminUsersApi
                .Patch(new AdminUsersPatchRequestContent
                {
                    IsApproved = true,
                    UserId = userId
                });
            DeleteSessionCookies();
        }

        public void ChangePassword(Guid userId, string oldPasswort, string newPassword)
        {
            _apiClient.ChangePasswordApi
                .Post(new ChangePasswordPostRequestContent {OldPassword = oldPasswort, NewPassword = newPassword});
        }

        public void ResetPassword(string emailAddress)
        {
            _apiClient.ResetPasswordApi
                .Post(new ResetPasswordPostRequestContent {EmailAddress = emailAddress});
        }

        public bool ChangeEmailAddress(Guid userId, string password, string newEmailAddress,
            bool assertStatusCode = true)
        {
            var response = _apiClient.Assert(assertStatusCode).ChangeEmailAddressApi
                .Post(new ChangeEmailAddressPostRequestContent {Password = password, NewEmailAddress = newEmailAddress});
            return response.StatusCode == HttpStatusCode.NoContent;
        }

        public void SetUsersRole(Guid userId, UserRole userRole)
        {
            LogInAsRoot();
            _apiClient.AdminUsersApi
                .Patch(new AdminUsersPatchRequestContent
                {
                    IsAdmin = true,
                    UserId = userId
                });
            DeleteSessionCookies();
        }

        public void LockUser(Guid userId)
        {
            LogInAsRoot();
            _apiClient.AdminUsersApi
                .Patch(new AdminUsersPatchRequestContent
                {
                    IsLocked = true,
                    UserId = userId
                });
            DeleteSessionCookies();
        }

        public void UnlockUser(Guid userId)
        {
            LogInAsRoot();
            _apiClient.AdminUsersApi
                .Patch(new AdminUsersPatchRequestContent
                {
                    IsLocked = false,
                    UserId = userId
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

        public Organization GetOrganization(Guid organizationId)
        {
            var response = _apiClient.OrganizationsApi
                .Get<Organization>(new {organizationId});
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

        public UsersGetOkResponseContent GetUser(Guid userId)
        {
            var response = _apiClient.UsersApi.Get<UsersGetOkResponseContent>(new {userId});
            return response.Data;
        }

        public Article CreateArticle(Guid ownerId, TableRow row = null)
        {
            var article = _fakeArticleGenerator.NextArticle(row);
            article.OwnerId = ownerId;
            var response = _apiClient.ArticlesApi.Post<ArticlesPostOkResponseContent>(new ArticlesPostRequestContent
            {
                Amount = article.GrossStock,
                Name = article.Name,
                OwnerId = article.OwnerId,
                PublicPrice = article.PublicPrice,
                MemberPrice = article.MemberPrice
            });
            article.ArticleId = response.Data.ArticleId;
            return article;
        }

        public QueryOkResponseContent<Article> QueryArticlesByUser(User user)
        {
            var response = _apiClient.ArticlesApi.Query<Article>(new {ownerId = user.Id});
            return response.Data;
        }

        public QueryOkResponseContent<Article> QueryArticlesByOrganization(Organization organization)
        {
            var response = _apiClient.ArticlesApi.Query<Article>(new {ownerId = organization.OrganizationId});
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
                .Get<UsersCartGetOkResponseContent>(new {userId = user.UserId});
            return response.Data;
        }

        public Guid ApplyForMembership(User user, Organization organization, bool assertHttpStatus = true)
        {
            var response = _apiClient.Assert(assertHttpStatus).OrganizationsApplicationsApi
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

        public Guid AddArticleToCart(User user, Article article, bool assertHttpStatus = true)
        {
            var response = _apiClient.Assert(assertHttpStatus).UserCartItemsApi
                .Post<UserCartItemsPostOkResponseContent>(new
                {
                    userId = user.UserId,
                    articleId = article.ArticleId,
                    quantity = 1,
                    fromUtc = DateTime.Today.Date.ToUniversalTime(),
                    toUtc = DateTime.Today.Date.AddDays(1).AddSeconds(-1).ToUniversalTime()
                });
            return response.Data.CartItemId;
        }

        public void RemoveCartItem(User user, Guid cartItemId)
        {
            _apiClient.UserCartItemsApi
                .Delete(new {userId = user.UserId, itemId = cartItemId});
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

        public void ChangeMembersRole(Guid organizationId, Guid memberId, MemberRole role)
        {
            _apiClient.OrganizationsMembersApi.Patch(
                new {organizationId, memberId, isManager = role == MemberRole.Manager});
        }

        public int CreateOrder(Guid organizationId, Guid lesseeId)
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
                FromUtc = DateTime.Today.Date.ToUniversalTime(),
                ToUtc = DateTime.Today.Date.AddDays(1).AddSeconds(-1).ToUniversalTime(),
                Quantity = 1
            });
        }

        public int PlaceOrder(User user, Guid lessorId, bool assertHttpStatus = true)
        {
            return _apiClient.Assert(assertHttpStatus).ShopOrdersApi
                .Post<ShopOrdersPostOkResponseContent>(new ShopOrdersPostRequestContent
                {
                    LessorId = lessorId
                })
                .Data.OrderId;
        }

        public void UpdateStartpage(Organization organization, string htmlContent)
        {
            _apiClient.OrganizationsApi.Patch(
                new {organizationId = organization.OrganizationId, startpage = htmlContent});
        }

        public FileUploadResponseContent UploadArticleImage(Article article, string fullFileName, string fileName = null)
        {
            return _apiClient.ArticlesFilesApi.PostFile<FileUploadResponseContent>(new {articleId = article.ArticleId},
                fullFileName, fileName);
        }

        public FileUploadResponseContent GetArticleFiles(Article article)
        {
            return _apiClient.ArticlesFilesApi.Get<FileUploadResponseContent>(new {articleId = article.ArticleId}).Data;
        }

        public void SetArticlePreviewImage(Article article, string fileName)
        {
            _apiClient.ArticlesFilesApi.Patch(new {articleId = article.ArticleId, fileName, isPreview = true});
        }

        public void ChangeOrganizationContactDetails(Organization organization, string postAddress, string phoneNumber,
            string emailAddress, string website)
        {
            _apiClient.OrganizationsApi.Patch(new
            {
                organizationId = organization.OrganizationId,
                contactDetails = new {postAddress, phoneNumber, emailAddress, website}
            });
        }

        public void SetMaintenanceMode(bool inMaintenance, bool assertStatusCode = true)
        {
            var response = _apiClient.Assert(assertStatusCode).MaintenanceApi.Patch(new {inMaintenance});
            _inMaintenanceMode = inMaintenance && response.StatusCode == HttpStatusCode.OK;
        }


        public Article GetArticle(Article article)
        {
            return _apiClient.ArticlesApi.Get<Article>(new {articleId = article.ArticleId},
                new {ownerId = article.OwnerId}).Data;
        }

        public Order GetOrder(int orderId)
        {
            return _apiClient.OrdersApi.Get<Order>(new {orderId}).Data;
        }

        public void ChangeArticlePrice(int articleId, decimal publicPrice, decimal? memberPrice)
        {
            _apiClient.ArticlesApi.Patch(new {articleId, prices = new {publicPrice, memberPrice}});
        }
    }
}
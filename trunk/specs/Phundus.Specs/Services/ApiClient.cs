namespace Phundus.Specs.Services
{
    using TechTalk.SpecFlow;

    [Binding]
    public class ApiClient
    {
        private bool _assertNextHttpStatusCode = true;

        public Resource StoresApi
        {
            get { return Resource("stores"); }
        }

        public Resource SessionsApi
        {
            get { return Resource("sessions"); }
        }

        public Resource AdminUsersApi
        {
            get { return Resource("admin/users/{userId}"); }
        }

        public Resource ResetPasswordApi
        {
            get { return Resource("account/reset-password"); }
        }

        public Resource ChangeEmailAddressApi
        {
            get { return Resource("account/change-email-address"); }
        }

        public Resource ChangePasswordApi
        {
            get { return Resource("account/change-password"); }
        }

        public Resource FeedbackApi
        {
            get { return Resource("feedback"); }
        }

        public Resource OrganizationsApi
        {
            get { return Resource("organizations/{organizationId}"); }
        }

        public Resource UsersApi
        {
            get { return Resource("users/{userId}"); }
        }

        public Resource ValidateApi
        {
            get { return Resource("account/validate"); }
        }

        public Resource MailsApi
        {
            get { return Resource("mails"); }
        }

        public Resource ArticlesApi
        {
            get { return Resource("articles/{articleId}"); }
        }

        public Resource StatusApi
        {
            get { return Resource("status"); }
        }

        public Resource UserCartApi
        {
            get { return Resource("users/{userId}/cart"); }
        }

        public Resource UserCartItemsApi
        {
            get { return Resource("users/{userId}/cart/items/{itemId}"); }
        }

        public Resource OrganizationsApplicationsApi
        {
            get { return Resource("organizations/{organizationId}/applications/{applicationId}"); }
        }

        public Resource OrganizationsRelationshipsApi
        {
            get { return Resource("organizations/{organizationId}/relationships"); }
        }

        public Resource OrganizationsMembersApi
        {
            get { return Resource("organizations/{organizationId}/members/{memberId}"); }
        }

        public Resource OrdersApi
        {
            get { return Resource("orders/{orderId}"); }
        }

        public Resource ShopItemsAvailabilityCheck
        {
            get { return Resource("shop/items/{itemId}/availability-check"); }
        }

        public Resource OrdersItemsApi
        {
            get { return Resource("orders/{orderId}/items/{orderItemId}"); }
        }

        public Resource ShopOrdersApi
        {
            get { return Resource("shop/orders"); }
        }

        public Resource ArticlesFilesApi
        {
            get { return Resource("articles/{articleId}/files/{fileName}"); }
        }

        public Resource MaintenanceApi
        {
            get { return Resource("maintenance"); }
        }

        public Resource OrganizationsSettingsApi
        {
            get { return Resource("organizations/{organizationId}/settings"); }
        }

        private Resource Resource(string url)
        {
            var result = new Resource(url, _assertNextHttpStatusCode);
            _assertNextHttpStatusCode = true;
            return result;
        }

        public void DeleteSessionCookies()
        {
            Services.Resource.DeleteSessionCookies();
        }

        public ApiClient Assert(bool assertStatusCode)
        {
            _assertNextHttpStatusCode = assertStatusCode;
            return this;
        }
    }
}
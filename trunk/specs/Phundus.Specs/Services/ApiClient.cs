namespace Phundus.Specs.Services
{
    using TechTalk.SpecFlow;

    [Binding]
    public class ApiClient
    {
        private static Resource Resource(string url)
        {
            return new Resource(url);
        }

        public void DeleteSessionCookies()
        {
            Services.Resource.DeleteSessionCookies();
        }

        public Resource SessionsApi()
        {
            return Resource("sessions");
        }

        public Resource AdminUsersApi()
        {
            return Resource("admin/users/{userGuid}");
        }

        public Resource ResetPasswordApi()
        {
            return Resource("account/reset-password");
        }

        public Resource ChangeEmailAddressApi()
        {
            return Resource("account/change-email-address");
        }

        public Resource ChangePasswordApi()
        {
            return Resource("account/change-password");
        }

        public Resource FeedbackApi()
        {
            return Resource("feedback");
        }

        public Resource OrganizationsApi()
        {
            return Resource("organizations/{organizationGuid}");
        }

        public Resource UsersApi()
        {
            return Resource("users");
        }

        public Resource ValidateApi()
        {
            return Resource("account/validate");
        }

        public Resource MailsApi()
        {
            return Resource("mails");
        }
    }
}
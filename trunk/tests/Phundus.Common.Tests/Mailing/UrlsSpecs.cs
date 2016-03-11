namespace Phundus.Common.Tests.Mailing
{
    using Common.Mailing;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;

    public class urls_concern : Observes<Urls>
    {
        protected static string serverUrl;

        private Establish ctx = () =>
            sut_factory.create_using(() => new Urls(serverUrl));
    }

    public class when_server_url_is_phundus_ch : urls_concern
    {
        private Establish ctx = () => serverUrl = @"phundus.ch";

        private It should_have_email_address_validation_with_https = () =>
            sut.UserEmailValidation.ShouldEqual(@"https://www.phundus.ch/#/validate/email-address");

        private It should_have_server_url_with_https = () =>
            sut.ServerUrl.ShouldEqual(@"https://www.phundus.ch/");

        private It should_have_user_account_validation_with_https = () =>
            sut.UserAccountValidation.ShouldEqual(@"https://www.phundus.ch/#/validate/account");
    }

    public class when_server_url_is_localhost : urls_concern
    {
        private Establish ctx = () => serverUrl = @"localhost";

        private It should_have_email_address_validation_without_https = () =>
            sut.UserEmailValidation.ShouldEqual(@"http://localhost/#/validate/email-address");

        private It should_have_server_url_without_https = () =>
            sut.ServerUrl.ShouldEqual("http://localhost/");

        private It should_have_user_account_Validation_without_https = () =>
            sut.UserAccountValidation.ShouldEqual(@"http://localhost/#/validate/account");
    }
}
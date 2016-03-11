namespace Phundus.Common.Tests.Mailing
{
    using Common.Mailing;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;

    public class urls_concern : Observes<Urls>
    {
        protected static string baseUrl = "http://host/";
        protected static string result;

        private Establish ctx = () =>
            sut_factory.create_using(() => new Urls(baseUrl));
    }

    [Subject(typeof (Urls))]
    public class when_make : urls_concern
    {
        private Because of = () =>
            result = sut.Make("/index.html");

        private It should_return_url_with_base_url = () =>
            result.ShouldEqual("http://host/index.html");
    }

    [Subject(typeof (Urls))]
    public class when_query_account_validation_without_key : urls_concern
    {
        private Because of = () =>
            result = sut.AccountValidation();

        private It should_return_url_without_key_param = () =>
            result.ShouldEqual("http://host/#/validate/account");
    }

    [Subject(typeof (Urls))]
    public class when_query_account_validation_with_key : urls_concern
    {
        private Because of = () =>
            result = sut.AccountValidation("123qwe");

        private It should_return_url_with_key_param = () =>
            result.ShouldEqual("http://host/#/validate/account?key=123qwe");
    }

    [Subject(typeof (Urls))]
    public class when_query_email_address_validation_without_key : urls_concern
    {
        private Because of = () =>
            result = sut.EmailAddressValidation();

        private It should_return_url_without_key_param = () =>
            result.ShouldEqual("http://host/#/validate/email-address");
    }

    [Subject(typeof (Urls))]
    public class when_query_email_address_validation_with_key : urls_concern
    {
        private Because of = () =>
            result = sut.EmailAddressValidation("123qwe");

        private It should_return_url_with_key_param = () =>
            result.ShouldEqual("http://host/#/validate/email-address?key=123qwe");
    }
}
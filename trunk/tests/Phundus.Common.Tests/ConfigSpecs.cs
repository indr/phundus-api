namespace Phundus.Common.Tests
{
    using System.Collections.Specialized;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;

    public class config_concern : Observes<ConfigImpl>
    {
        protected static NameValueCollection settings;
        protected static string serverUrl;

        private Establish ctx = () =>
            sut_factory.create_using(() =>
            {
                settings = new NameValueCollection();
                settings["serverUrl"] = serverUrl;
                return new ConfigImpl(settings);
            });
    }

    [Subject(typeof (ConfigImpl))]
    public class when_empty_server_url : config_concern
    {
        private Establish ctx = () =>
            serverUrl = "";

        private It should_have_base_url_http_localhost = () =>
            sut.BaseUrl.ShouldEqual("http://localhost/");

        private It should_have_server_url_localhost = () =>
            sut.ServerUrl.ShouldEqual("localhost");
    }

    [Subject(typeof (ConfigImpl))]
    public class when_server_url_is_localhost : config_concern
    {
        private Establish ctx = () =>
            serverUrl = "localhost";

        private It should_have_base_url_http_localhost = () =>
            sut.BaseUrl.ShouldEqual("http://localhost/");

        private It should_have_server_url_localhost = () =>
            sut.ServerUrl.ShouldEqual("localhost");
    }

    [Subject(typeof (ConfigImpl))]
    public class when_server_url_is_acceptance_test_phundus_ch : config_concern
    {
        private Establish ctx = () =>
            serverUrl = "acceptance.test.phundus.ch";

        private It should_have_base_url_http_acceptance_test_phundus_ch = () =>
            sut.BaseUrl.ShouldEqual("http://acceptance.test.phundus.ch/");

        private It should_have_server_url_acceptance_test_phundus_ch = () =>
            sut.ServerUrl.ShouldEqual("acceptance.test.phundus.ch");
    }

    [Subject(typeof(ConfigImpl))]
    public class when_server_url_is_staging_test_phundus_ch : config_concern
    {
        private Establish ctx = () =>
            serverUrl = "staging.test.phundus.ch";

        private It should_have_base_url_http_staging_test_phundus_ch = () =>
            sut.BaseUrl.ShouldEqual("http://staging.test.phundus.ch/");

        private It should_have_server_url_staging_test_phundus_ch = () =>
            sut.ServerUrl.ShouldEqual("staging.test.phundus.ch");
    }

    [Subject(typeof(ConfigImpl))]
    public class when_server_url_is_phundus_ch : config_concern
    {
        private Establish ctx = () =>
            serverUrl = "phundus.ch";

        private It should_have_base_url_https_www_phundus_ch = () =>
            sut.BaseUrl.ShouldEqual("https://www.phundus.ch/");

        private It should_have_server_url_www_phundus_ch = () =>
            sut.ServerUrl.ShouldEqual("www.phundus.ch");
    }

    [Subject(typeof(ConfigImpl))]
    public class when_server_url_is_www_phundus_ch : config_concern
    {
        private Establish ctx = () =>
            serverUrl = "www.phundus.ch";

        private It should_have_base_url_https_www_phundus_ch = () =>
            sut.BaseUrl.ShouldEqual("https://www.phundus.ch/");

        private It should_have_server_url_www_phundus_ch = () =>
            sut.ServerUrl.ShouldEqual("www.phundus.ch");
    }
}
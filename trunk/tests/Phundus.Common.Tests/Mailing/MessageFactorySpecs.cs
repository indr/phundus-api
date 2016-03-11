namespace Phundus.Common.Tests.Mailing
{
    using System;
    using System.Net.Mail;
    using Common.Mailing;
    using developwithpassion.specifications.extensions;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;
    using Rhino.Mocks;

    public class when_creating_with_empty_subject_template : Observes
    {
        private Because of = () =>
            spec.catch_exception(() =>
                new MessageFactory(fake.an<IModelFactory>(), "", "text", "body"));

        private It should_throw_argument_exception = () =>
            spec.exception_thrown.ShouldBeOfExactType<ArgumentNullException>();

        private It should_throw_with_parameter_subject_template = () =>
            (spec.exception_thrown as ArgumentNullException).ParamName.ShouldEqual("subject");
    }

    public class when_creating_with_empty_text_body_and_empty_html_body : Observes
    {
        private Because of = () =>
            spec.catch_exception(() =>
                new MessageFactory(fake.an<IModelFactory>(), "Subject", "", ""));

        private It should_throw_argument_exception = () =>
            spec.exception_thrown.ShouldBeOfExactType<ArgumentNullException>();

        private It should_throw_with_parameter_subject_template = () =>
            (spec.exception_thrown as ArgumentNullException).ParamName.ShouldEqual("textBody");
    }

    public class make_mail_message_concern : Observes<MessageFactory>
    {
        protected static string subjectTemplate;
        protected static string textBodyTemplate;
        protected static string htmlBodyTemplate;
        protected static dynamic data;
        protected static MailMessage message;

        protected static IModelFactory modelFactory;
        private static object model;

        private Establish ctx = () =>
        {
            subjectTemplate = "Subject template";
            textBodyTemplate = "Text body template";
            htmlBodyTemplate = "Html body template";


            data = new
            {
                FirstName = "John",
                LastName = "Doe"
            };
            model = new
            {
                Urls = new
                {
                    ServerUrl = ""
                },
                FirstName = "John",
                LastName = "Doe"
            };
            modelFactory = depends.on<IModelFactory>();
            modelFactory.setup(x => x.MakeModel(Arg<object>.Is.Anything)).Return(model);
            sut_factory.create_using(() =>
                new MessageFactory(modelFactory, subjectTemplate, textBodyTemplate, htmlBodyTemplate));
        };

        private Because of = () =>
            message = sut.MakeMessage(data);
    }


    public class generating_mail_message_with_subject : make_mail_message_concern
    {
        private Establish ctx = () => { subjectTemplate = "Hello @Model.FirstName @Model.LastName"; };

        private It should_generate_subject = () =>
            message.Subject.ShouldContain("Hello John Doe");

        private It should_generate_subject_with_prefix = () =>
            message.Subject.ShouldStartWith("[phundus] ");
    }

    public class generating_mail_message_with_text_body : make_mail_message_concern
    {
        private Establish ctx = () =>
        {
            textBodyTemplate = @"Text body for @Model.FirstName @Model.LastName";
            htmlBodyTemplate = null;
        };

        private It should_have_is_body_html_false = () =>
            message.IsBodyHtml.ShouldBeFalse();

        private It should_make_body_with_signature = () =>
            message.Body.ShouldEndWith(MailTemplates.TextSignature);

        private It should_parse_body = () =>
            message.Body.ShouldStartWith(@"Text body for John Doe");
    }

    public class generating_mail_message_with_html_body : make_mail_message_concern
    {
        private Establish ctx = () =>
        {
            textBodyTemplate = null;
            htmlBodyTemplate = @"Html body for @Model.FirstName @Model.LastName";
        };

        private It should_add_html_footer = () =>
            message.Body.ShouldEndWith(@"</body>
</html>
");

        private It should_add_html_header = () =>
            message.Body.ShouldStartWith(@"<!DOCTYPE html PUBLIC ");

        private It should_have_is_body_html_true = () =>
            message.IsBodyHtml.ShouldBeTrue();

        private It should_parse_body = () =>
            message.Body.ShouldContain(@"Html body for John Doe");
    }

    public class generating_mail_message_with_text_body_and_html_body : make_mail_message_concern
    {
        private Establish ctx = () =>
        {
            textBodyTemplate = @"Text body for @Model.FirstName @Model.LastName";
            htmlBodyTemplate = @"Html body for @Model.FirstName @Model.LastName";
        };

        private It shold_have_a_text_alternative_view = () =>
            message.AlternateViews.ShouldContain(p => p.ContentType.MediaType == ContentTypes.Text);

        private It should_have_an_html_alternative_view = () =>
            message.AlternateViews.ShouldContain(p => p.ContentType.MediaType == ContentTypes.Html);

        private It should_have_empty_body = () =>
            message.Body.ShouldBeEmpty();

        private It should_have_is_body_html_false = () =>
            message.IsBodyHtml.ShouldBeFalse();

        private It should_have_two_alternative_views = () =>
            message.AlternateViews.Count.ShouldEqual(2);
    }
}
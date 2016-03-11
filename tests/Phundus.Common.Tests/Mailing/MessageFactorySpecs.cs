namespace Phundus.Common.Tests.Mailing
{
    using System;
    using System.Net.Mail;
    using Common.Mailing;
    using developwithpassion.specifications.extensions;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;
    using Rhino.Mocks;

    public class TestModel : MailModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class make_mail_message_concern : Observes<MessageFactory>
    {
        protected static string subjectTemplate;
        protected static string textBodyTemplate;
        protected static string htmlBodyTemplate;
        protected static object model;
        protected static MailMessage message;

        private Establish ctx = () =>
        {
            subjectTemplate = "Subject template";
            textBodyTemplate = "Text body template";
            htmlBodyTemplate = "Html body template";

            model = new TestModel
            {
                FirstName = "John",
                LastName = "Doe"
            };
        };
    }

    [Subject(typeof (MessageFactory))]
    public class generating_mail_message_with_empty_subject_template : make_mail_message_concern
    {
        private Establish ctx = () =>
            subjectTemplate = "";

        private Because of = () =>
            spec.catch_exception(() =>
                message = sut.MakeMessage(model, subjectTemplate, textBodyTemplate, htmlBodyTemplate));

        private It should_throw_argument_exception = () =>
            spec.exception_thrown.ShouldBeOfExactType<ArgumentNullException>();

        private It should_throw_with_parameter_subject_template = () =>
            (spec.exception_thrown as ArgumentNullException).ParamName.ShouldEqual("subject");
    }

    [Subject(typeof (MessageFactory))]
    public class generating_mail_message_with_empty_text_body_and_empty_html_body : make_mail_message_concern
    {
        private Establish ctx = () =>
            htmlBodyTemplate = textBodyTemplate = "";

        private Because of = () =>
            spec.catch_exception(() =>
                message = sut.MakeMessage(model, subjectTemplate, textBodyTemplate, htmlBodyTemplate));

        private It should_throw_argument_exception = () =>
            spec.exception_thrown.ShouldBeOfExactType<ArgumentNullException>();

        private It should_throw_with_parameter_subject_template = () =>
            (spec.exception_thrown as ArgumentNullException).ParamName.ShouldEqual("textBody");
    }


    [Subject(typeof (MessageFactory))]
    public class generating_mail_message_with_subject : make_mail_message_concern
    {
        private Establish ctx = () => { subjectTemplate = "Hello @Model.FirstName @Model.LastName"; };

        private Because of = () =>
            message = sut.MakeMessage(model, subjectTemplate, textBodyTemplate, htmlBodyTemplate);

        private It should_generate_subject = () =>
            message.Subject.ShouldContain("Hello John Doe");

        private It should_generate_subject_with_prefix = () =>
            message.Subject.ShouldStartWith("[phundus] ");
    }

    [Subject(typeof (MessageFactory))]
    public class generating_mail_message_with_text_body : make_mail_message_concern
    {
        private Establish ctx = () =>
        {
            textBodyTemplate = @"Text body for @Model.FirstName @Model.LastName";
            htmlBodyTemplate = null;
        };

        private Because of = () =>
            message = sut.MakeMessage(model, subjectTemplate, textBodyTemplate, htmlBodyTemplate);

        private It should_have_is_body_html_false = () =>
            message.IsBodyHtml.ShouldBeFalse();

        private It should_make_body_with_signature = () =>
            message.Body.ShouldEndWith(MailTemplates.TextSignature);

        private It should_parse_body = () =>
            message.Body.ShouldStartWith(@"Text body for John Doe");
    }

    [Subject(typeof (MessageFactory))]
    public class generating_mail_message_with_html_body : make_mail_message_concern
    {
        private Establish ctx = () =>
        {
            textBodyTemplate = null;
            htmlBodyTemplate = @"Html body for @Model.FirstName @Model.LastName";
        };

        private Because of = () =>
            message = sut.MakeMessage(model, subjectTemplate, textBodyTemplate, htmlBodyTemplate);

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

    [Subject(typeof (MessageFactory))]
    public class generating_mail_message_with_text_body_and_html_body : make_mail_message_concern
    {
        private Establish ctx = () =>
        {
            textBodyTemplate = @"Text body for @Model.FirstName @Model.LastName";
            htmlBodyTemplate = @"Html body for @Model.FirstName @Model.LastName";
        };

        private Because of = () =>
            message = sut.MakeMessage(model, subjectTemplate, textBodyTemplate, htmlBodyTemplate);

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
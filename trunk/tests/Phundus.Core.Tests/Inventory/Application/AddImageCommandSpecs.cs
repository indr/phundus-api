namespace Phundus.Tests.Inventory.Application
{
    using System;
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Inventory.Application;

    [Subject(typeof (AddImage))]
    public class when_instantiating_add_image_with_path_in_file_name
    {
        private static Exception caughtException;

        private Because of = () => caughtException = Catch.Exception(() =>
            new AddImage(new InitiatorId(), new ArticleShortId(1), @"Path\To\File.jpg", "image/jpeg", 1));

        private It should_throw_argument_exception = () =>
            caughtException.ShouldBeOfExactType<ArgumentException>();

        private It should_throw_exception_message = () =>
            caughtException.Message.ShouldStartWith(@"The file name ""Path\To\File.jpg"" contains invalid characters. Did you mistakenly provide path information?");
    }
}
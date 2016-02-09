namespace Phundus.Tests.Inventory.Articles.Commands
{
    using System;
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Inventory.Articles.Commands;

    [Subject(typeof(RemoveImage))]
    public class when_instantiating_remove_image_with_path_in_file_name
    {
        private static Exception caughtException;

        private Because of = () => caughtException = Catch.Exception(() =>
            new RemoveImage(new InitiatorId(), new ArticleShortId(1), @"Path\To\File.jpg"));

        private It should_throw_argument_exception = () =>
            caughtException.ShouldBeOfExactType<ArgumentException>();

        private It should_throw_exception_message = () =>
            caughtException.Message.ShouldStartWith(@"The file name ""Path\To\File.jpg"" contains invalid characters. Did you mistakenly provide path information?");
    }
}
namespace Phundus.Tests.Inventory.Application
{
    using System;
    using Common.Domain.Model;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;
    using Phundus.Inventory.Application;

    [Subject(typeof (RemoveImage))]
    public class when_instantiating_remove_image_with_path_in_file_name : Observes
    {
        private Because of = () =>
            spec.catch_exception(() =>
                new RemoveImage(new InitiatorId(), new ArticleId(), @"Path\To\File.jpg"));

        private It should_throw_argument_exception = () =>
            spec.exception_thrown.ShouldBeOfExactType<ArgumentException>();

        private It should_throw_exception_message = () =>
            spec.exception_thrown.Message.ShouldStartWith(
                @"The file name ""Path\To\File.jpg"" contains invalid characters. Did you mistakenly provide path information?");
    }
}
namespace Phundus.Tests.Inventory.Application
{
    using System;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;
    using Phundus.Inventory.Application;

    [Subject(typeof (AddImage))]
    public class when_instantiating_add_image_with_path_in_file_name : Observes
    {
        private Because of = () =>
            spec.catch_exception(() =>
                new AddImage(new InitiatorId(), new ArticleId(), @"Path\To\File.jpg", "image/jpeg", 1));

        private It should_throw_argument_exception = () =>
            spec.exception_thrown.ShouldBeAn<ArgumentException>();

        private It should_throw_exception_message = () =>
            spec.exception_thrown.Message.ShouldStartWith(
                @"The file name ""Path\To\File.jpg"" contains invalid characters. Did you mistakenly provide path information?");
    }

    [Subject(typeof (AddImage))]
    public class when_instantiating_add_image_with_sile_size_0 : Observes
    {
        private Because of = () =>
            spec.catch_exception(() =>
                new AddImage(new InitiatorId(), new ArticleId(), "file.jpg", "image/jpeg", 0));

        private It should_throw_argument_out_of_range_exception = () =>
            spec.exception_thrown.ShouldBeAn<ArgumentOutOfRangeException>();
    }
}
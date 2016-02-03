﻿namespace Phundus.Tests.Inventory.Events
{
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Inventory.Articles.Model;

    [Subject(typeof (ImageRemoved))]
    public class ImageRemovedSpecs : domain_event_concern<ImageRemoved>
    {
        private static ArticleId theArticleId;
        private static ArticleGuid theArticleGuid;
        private static OwnerId theOwnerId;
        private static string theFileName;

        private Establish ctx = () =>
        {
            theArticleId = new ArticleId(1234);
            theArticleGuid = new ArticleGuid();
            theOwnerId = new OwnerId();
            theFileName = "/path/to/filename.type";

            sut_factory.create_using(() => new ImageRemoved(theInitiator, theArticleId, theArticleGuid, theOwnerId, theFileName));
        };

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.Inventory.Articles.Model.ImageRemoved");

        private It should_have_at_1_the_initiator = () =>
            dataMember(1).ShouldEqual(theInitiator);

        private It should_have_at_2_the_article_integral_id = () =>
            dataMember(2).ShouldEqual(theArticleId.Id);

        private It should_have_at_3_the_article_guid = () =>
            dataMember(3).ShouldEqual(theArticleGuid.Id);

        private It should_have_at_4_the_owner_id = () =>
            dataMember(4).ShouldEqual(theOwnerId.Id);

        private It should_have_at_5_the_file_name = () =>
            dataMember(5).ShouldEqual(theFileName);

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");
    }
}
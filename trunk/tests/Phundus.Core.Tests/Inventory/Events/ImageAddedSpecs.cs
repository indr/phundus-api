namespace Phundus.Tests.Inventory.Events
{
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Inventory.Articles.Model;

    [Subject(typeof (ImageAdded))]
    public class ImageAddedSpecs : domain_event_concern<ImageAdded>
    {
        private static ArticleId theArticleId;
        private static ArticleGuid theArticleGuid;
        private static OwnerId theOwnerId;
        private static string theFileName;
        private static string theFileType;
        private static long theFileLength;
        private static bool theIsPreviewImage;

        private Establish ctx = () =>
        {
            theArticleId = new ArticleId(1234);
            theArticleGuid = new ArticleGuid();
            theOwnerId = new OwnerId();
            theFileName = "/path/to/filename.type";
            theFileType = "image/jpeg";
            theFileLength = 1234567;
            theIsPreviewImage = true;

            sut_factory.create_using(() => new ImageAdded(theInitiator, theArticleId, theArticleGuid, theOwnerId,
                theFileName, theFileType, theFileLength, theIsPreviewImage));
        };

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

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

        private It should_have_at_6_the_file_type = () =>
            dataMember(6).ShouldEqual(theFileType);

        private It should_have_at_7_the_file_length = () =>
            dataMember(7).ShouldEqual(theFileLength);

        private It should_have_at_8_is_preview_image = () =>
            dataMember(8).ShouldEqual(theIsPreviewImage);

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.Inventory.Articles.Model.ImageAdded");
    }
}
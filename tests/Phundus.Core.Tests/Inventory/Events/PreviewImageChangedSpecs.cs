namespace Phundus.Tests.Inventory.Events
{
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Inventory.Articles.Model;

    [Subject(typeof (PreviewImageChanged))]
    public class PreviewImageChangedSpecs : inventory_domain_event_concern<PreviewImageChanged>
    {
        private static ArticleShortId the_article_short_id;
        private static ArticleId the_article_id;
        private static OwnerId theOwnerId;
        private static string theFileName;
        private static string theFileType;
        private static long theFileLength;

        private Establish ctx = () =>
        {
            the_article_short_id = new ArticleShortId(1234);
            the_article_id = new ArticleId();
            theOwnerId = new OwnerId();
            theFileName = "/path/to/filename.type";
            theFileType = "image/jpeg";
            theFileLength = 1234567;

            sut_factory.create_using(() => new PreviewImageChanged(theManager, the_article_short_id, the_article_id, theOwnerId,
                theFileName, theFileType, theFileLength));
        };

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.Inventory.Articles.Model.PreviewImageChanged");

        private It should_have_at_1_the_initiator = () =>
            dataMember(1).ShouldEqual(theManager.ToActor());

        private It should_have_at_2_the_article_integral_id = () =>
            dataMember(2).ShouldEqual(the_article_short_id.Id);

        private It should_have_at_3_the_article_guid = () =>
            dataMember(3).ShouldEqual(the_article_id.Id);

        private It should_have_at_4_the_owner_id = () =>
            dataMember(4).ShouldEqual(theOwnerId.Id);

        private It should_have_at_5_the_file_name = () =>
            dataMember(5).ShouldEqual(theFileName);

        private It should_have_at_6_the_file_type = () =>
            dataMember(6).ShouldEqual(theFileType);

        private It should_have_at_7_the_file_length = () =>
            dataMember(7).ShouldEqual(theFileLength);

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");
    }
}
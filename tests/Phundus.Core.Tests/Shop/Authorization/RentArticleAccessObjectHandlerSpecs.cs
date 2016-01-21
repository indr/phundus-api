namespace Phundus.Tests.Shop.Authorization
{
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Shop.Authorization;
    using Phundus.Shop.Orders.Model;


    [Subject(typeof (RentArticleAccessObjectHandler))]
    public class when_handling_rent_article_access_request :
        shop_access_object_handler_concern<RentArticle, RentArticleAccessObjectHandler>
    {
        private static Article theArticle;

        private Establish ctx = () =>
        {
            theArticle = make.ShopArticle();
            theAccessObject = new RentArticle(theArticle);
        };

        private It should_ask_for_member_privileges = () =>
            memberInRole.WasToldTo(x => x.ActiveMember(theArticle.LessorId.Id, theUserId));
    }
}
namespace Phundus.Tests.Shop.Authorization
{
    using Common.Domain.Model;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Authorization;
    using Phundus.IdentityAccess.Queries;
    using Phundus.Shop.Authorization;
    using Phundus.Shop.Orders.Model;

    public class access_object_handler_concern<TAccessObject, TAccessObjectHandler> : Observes<TAccessObjectHandler>
        where TAccessObjectHandler : class, IHandleAccessObject<TAccessObject>
    {
        protected static UserId theUserId;
        protected static TAccessObject theAccessObject;

        private Because of = () =>
            sut.Handle(theUserId, theAccessObject);
    }

    public class shop_access_object_handler_concern<TAccessObject, TAccessObjectHandler> :
        access_object_handler_concern<TAccessObject, TAccessObjectHandler>
        where TAccessObjectHandler : class, IHandleAccessObject<TAccessObject>
    {
        protected static shop_factory make;
        protected static IMemberInRole memberInRole;

        private Establish ctx = () =>
        {
            memberInRole = depends.on<IMemberInRole>();
            make = new shop_factory(fake);
        };
    }


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
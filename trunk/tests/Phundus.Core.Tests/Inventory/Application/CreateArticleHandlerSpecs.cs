namespace Phundus.Tests.Inventory.Application
{
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Inventory.Application;
    using Phundus.Inventory.Articles.Model;
    using Phundus.Inventory.Authorization;
    using Phundus.Inventory.Model;
    using Phundus.Inventory.Stores.Model;
    using Phundus.Inventory.Stores.Repositories;
    using Rhino.Mocks;

    [Subject(typeof (CreateArticleHandler))]
    public class when_create_article_command_is_handled :
        article_command_handler_concern<CreateArticle, CreateArticleHandler>
    {
        private static Owner theOwner;
        private static ArticleId theArticleId;
        private static string theName;
        private static int theGrossStock;
        private static decimal theMemberPrice;
        private static decimal thePublicPrice;
        private static ArticleShortId theArticleShortId;
        private static Store theStore;

        public Establish c = () =>
        {
            theOwner = make.Owner();
            theStore = make.Store(theOwner);
            theArticleId = new ArticleId();
            theArticleShortId = new ArticleShortId(1234);
            theName = "The name";
            theGrossStock = 10;
            theMemberPrice = 11.10m;
            thePublicPrice = 12.20m;

            var idProperty = typeof (Article).GetProperty("Id");
            articleRepository.Expect(x => x.Add(Arg<Article>.Is.NotNull)).WhenCalled(a =>
                idProperty.SetValue(a.Arguments[0], theArticleShortId.Id, null));

            ownerService.setup(x => x.GetById(theOwner.OwnerId)).Return(theOwner);
            depends.on<IStoreRepository>().setup(x => x.GetById(theStore.StoreId)).Return(theStore);

            command = new CreateArticle(theInitiatorId, theOwner.OwnerId, theStore.StoreId, theArticleId,
                theArticleShortId, theName, theGrossStock, thePublicPrice, theMemberPrice);
        };

        private It should_add_to_repository = () =>
            articleRepository.WasToldTo(x => x.Add(Arg<Article>.Is.NotNull));

        private It should_authorize_initiator_to_create_article = () =>
            authorize.WasToldTo(x =>
                x.Enforce(Arg<InitiatorId>.Is.Equal(theInitiatorId),
                    Arg<CreateArticleAccessObject>.Matches(p => Equals(p.OwnerId, theOwner.OwnerId))));

        public It should_publish_article_created = () =>
            published<ArticleCreated>(p =>
                p.ArticleId == theArticleId.Id
                && p.ArticleShortId == theArticleShortId.Id
                && p.GrossStock == theGrossStock
                && Equals(p.Initiator.InitiatorId, theInitiatorId)
                && p.MemberPrice == theMemberPrice
                && p.Name == theName
                && Equals(p.Owner.OwnerId, theOwner.OwnerId)
                && p.PublicPrice == thePublicPrice
                && p.StoreId == theStore.StoreId.Id
                && p.StoreName == theStore.Name);
    }
}
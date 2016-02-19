namespace Phundus.Tests.Inventory.Articles.Commands
{
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Inventory.Articles.Commands;
    using Phundus.Inventory.Articles.Model;
    using Phundus.Inventory.Authorization;
    using Phundus.Inventory.Stores.Model;
    using Phundus.Inventory.Stores.Repositories;
    using Rhino.Mocks;

    [Subject(typeof (CreateArticleHandler))]
    public class when_create_article_command_is_handled :
        article_command_handler_concern<CreateArticle, CreateArticleHandler>
    {
        private static Owner theOwner;
        private static ArticleId the_article_id;
        private static string theName;
        private static int theGrossStock;
        private static decimal theMemberPrice;
        private static decimal thePublicPrice;
        private static int theArticleId;
        private static Store theStore;

        public Establish c = () =>
        {
            theOwner = make.Owner();
            theStore = make.Store();
            the_article_id = new ArticleId();
            theName = "The name";
            theGrossStock = 10;
            theMemberPrice = 11.10m;
            thePublicPrice = 12.20m;

            articleRepository.setup(x => x.Add(Arg<Article>.Is.Anything)).Return(theArticleId);
            ownerService.setup(x => x.GetById(theOwner.OwnerId)).Return(theOwner);
            depends.on<IStoreRepository>().setup(x => x.GetByOwnerAndId(theOwner.OwnerId, theStore.Id)).Return(theStore);

            command = new CreateArticle(theInitiatorId, theOwner.OwnerId, theStore.Id, the_article_id,
                theName, theGrossStock, thePublicPrice, theMemberPrice);
        };

        private It should_add_to_repository = () => articleRepository.WasToldTo(x => x.Add(Arg<Article>.Is.NotNull));

        private It should_authorize_initiator_to_create_article = () =>
            authorize.WasToldTo(x =>
                x.Enforce(Arg<InitiatorId>.Is.Equal(theInitiatorId),
                    Arg<CreateArticleAccessObject>.Matches(p => Equals(p.OwnerId, theOwner.OwnerId))));

        public It should_publish_article_created = () =>
            Published<ArticleCreated>(p =>
                p.ArticleId == the_article_id.Id
                && p.GrossStock == theGrossStock
                && p.Initiator.InitiatorGuid == theInitiatorId.Id
                && p.MemberPrice == theMemberPrice
                && p.Name == theName
                && Equals(p.Owner.OwnerId, theOwner.OwnerId)
                && p.PublicPrice == thePublicPrice
                && p.StoreId == theStore.Id.Id);

        public It should_set_resulting_article_id = () => command.ResultingArticleId.ShouldEqual(theArticleId);
    }
}
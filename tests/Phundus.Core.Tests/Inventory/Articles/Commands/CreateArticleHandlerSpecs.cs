namespace Phundus.Tests.Inventory.Articles.Commands
{
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Inventory.Articles.Commands;
    using Phundus.Inventory.Articles.Model;
    using Phundus.Inventory.Stores.Model;
    using Phundus.Inventory.Stores.Repositories;
    using Rhino.Mocks;

    [Subject(typeof (CreateArticleHandler))]
    public class when_create_article_command_is_handled :
        article_command_handler_concern<CreateArticle, CreateArticleHandler>
    {
        private static Owner theOwner;
        private static ArticleGuid theArticleGuid;
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
            theArticleGuid = new ArticleGuid();
            theName = "The name";
            theGrossStock = 10;
            theMemberPrice = 11.10m;
            thePublicPrice = 12.20m;

            articleRepository.setup(x => x.Add(Arg<Article>.Is.Anything)).Return(theArticleId);
            ownerService.setup(x => x.GetById(theOwner.OwnerId)).Return(theOwner);
            depends.on<IStoreRepository>().setup(x => x.GetByOwnerAndId(theOwner.OwnerId, theStore.Id)).Return(theStore);

            command = new CreateArticle(theInitiatorId, theOwner.OwnerId, theStore.Id, theArticleGuid,
                theName, theGrossStock, thePublicPrice, theMemberPrice);
        };

        public It should_add_to_repository = () => articleRepository.WasToldTo(x => x.Add(Arg<Article>.Is.NotNull));

        public It should_ask_for_chief_privileges =
            () => memberInRole.WasToldTo(x => x.ActiveManager(theOwner.OwnerId, theInitiatorId));

        public It should_publish_article_created =
            () => publisher.WasToldTo(x => x.Publish(Arg<ArticleCreated>.Matches(p =>
                p.ArticleGuid == theArticleGuid.Id
                && p.GrossStock == theGrossStock
                && p.Initiator.InitiatorGuid == theInitiatorId.Id
                && p.MemberPrice == theMemberPrice
                && p.Name == theName
                && Equals(p.Owner.OwnerId, theOwner.OwnerId)
                && p.PublicPrice == thePublicPrice
                && p.StoreId == theStore.Id.Id)));

        public It should_set_article_id = () => command.ResultingArticleId.ShouldNotBeNull();
    }
}
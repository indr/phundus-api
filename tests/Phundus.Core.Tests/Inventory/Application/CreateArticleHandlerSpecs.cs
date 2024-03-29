﻿namespace Phundus.Tests.Inventory.Application
{
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Inventory.Application;
    using Phundus.Inventory.Articles.Model;
    using Phundus.Inventory.Model;
    using Phundus.Inventory.Model.Stores;
    using Rhino.Mocks;

    [Subject(typeof (CreateArticleHandler))]
    public class when_create_article_command_is_handled :
        article_command_handler_concern<CreateArticle, CreateArticleHandler>
    {        
        private static ArticleId theArticleId;
        private static string theName;
        private static int theGrossStock;
        private static decimal theMemberPrice;
        private static decimal thePublicPrice;
        private static ArticleShortId theArticleShortId;
        private static Store theStore;

        public Establish c = () =>
        {            
            theStore = make.Store(theOwner.OwnerId);
            theArticleId = new ArticleId();
            theArticleShortId = new ArticleShortId(1234);
            theName = "The name";
            theGrossStock = 10;
            theMemberPrice = 11.10m;
            thePublicPrice = 12.20m;

            ownerService.setup(x => x.GetById(theOwner.OwnerId)).Return(theOwner);
            depends.on<IStoreRepository>().setup(x => x.GetById(theStore.StoreId)).Return(theStore);

            command = new CreateArticle(theInitiatorId, theOwner.OwnerId, theStore.StoreId, theArticleId,
                theArticleShortId, theName, theGrossStock, thePublicPrice, theMemberPrice);
        };

        private It should_add_to_repository = () =>
            articleRepository.WasToldTo(x => x.Add(Arg<Article>.Is.NotNull));

        public It should_publish_article_created = () =>
            published<ArticleCreated>(p =>
                p.ArticleId == theArticleId.Id
                && p.ArticleShortId == theArticleShortId.Id
                && p.GrossStock == theGrossStock
                && Equals(p.Initiator, theManager.ToActor())
                && p.MemberPrice == theMemberPrice
                && p.Name == theName
                && Equals(p.Owner.OwnerId, theOwner.OwnerId)
                && p.PublicPrice == thePublicPrice
                && p.StoreId == theStore.StoreId.Id
                && p.StoreName == theStore.Name);
    }
}
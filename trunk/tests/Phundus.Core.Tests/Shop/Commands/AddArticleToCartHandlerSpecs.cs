namespace Phundus.Tests.Shop.Commands
{
    using System;
    using Common.Domain.Model;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Authorization;
    using Phundus.IdentityAccess.Queries;
    using Phundus.Shop.Authorization;
    using Phundus.Shop.Orders.Commands;
    using Phundus.Shop.Orders.Model;
    using Phundus.Shop.Orders.Repositories;
    using Phundus.Shop.Services;
    using Rhino.Mocks;

    public class when_add_article_to_cart_is_handled : handler_concern<AddArticleToCart, AddArticleToCartHandler>
    {
        protected const int theQuantity = 3;
        protected static readonly ArticleId theArticleId = new ArticleId(12345);
        protected static readonly DateTime theFromUtc = DateTime.UtcNow;
        protected static readonly DateTime theToUtc = DateTime.UtcNow.AddDays(1);
        protected static IMemberInRole memberInRole;
        protected static Article theArticle;
        protected static readonly OwnerId theOwnerId = new OwnerId();
        protected static ICartRepository cartRepository;

        protected static IAuthorize authorize;

        private Establish ctx =
            () =>
            {
                theArticle = new Article(theArticleId.Id, new Owner(theOwnerId, "Owner"), "Article", 7);
                cartRepository = depends.on<ICartRepository>();
                memberInRole = depends.on<IMemberInRole>();

                authorize = depends.on<IAuthorize>();


                depends.on<IArticleService>().WhenToldTo(x => x.GetById(theArticleId)).Return(theArticle);
                command = new AddArticleToCart(theInitiatorId, theArticleId, theFromUtc, theToUtc, theQuantity);
            };
    }

    [Subject(typeof (AddArticleToCartHandler))]
    public class when_user_has_no_cart_yet : when_add_article_to_cart_is_handled
    {
        private Establish ctx =
            () => cartRepository.WhenToldTo(x => x.FindByUserGuid(theInitiatorId)).Return((Cart) null);

        private It should_add_new_cart_to_repository = () => cartRepository.WasToldTo(x => x.Add(Arg<Cart>.Is.NotNull));

        private It should_authorize_user_to_rent_article = () => authorize.WasToldTo(x =>
            x.User(Arg<InitiatorId>.Is.Equal(command.InitiatorId),
                Arg<RentArticle>.Matches(p => Equals(p.Article, theArticle))));
    }

    [Subject(typeof (AddArticleToCartHandler))]
    public class when_user_already_has_a_cart : when_add_article_to_cart_is_handled
    {
        private static Cart theCart;

        private Establish ctx = () =>
        {
            theCart = new Cart(theInitiatorId, theInitiatorId);
            cartRepository.WhenToldTo(x => x.FindByUserGuid(theInitiatorId)).Return(theCart);
        };

        private It should_authorize_user_to_rent_article = () => authorize.WasToldTo(x =>
            x.User(Arg<InitiatorId>.Is.Equal(command.InitiatorId),
                Arg<RentArticle>.Matches(p => Equals(p.Article, theArticle))));

        private It should_not_add_a_cart_to_repository =
            () => cartRepository.WasNotToldTo(x => x.Add(Arg<Cart>.Is.Anything));
    }
}
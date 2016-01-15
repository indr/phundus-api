namespace Phundus.Tests.Shop.Commands
{
    using System;
    using Common.Domain.Model;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Queries;
    using Phundus.Shop.Orders.Commands;
    using Phundus.Shop.Orders.Model;
    using Phundus.Shop.Orders.Repositories;
    using Phundus.Shop.Services;
    using Rhino.Mocks;

    public class when_add_article_to_cart_is_handled : handler_concern<AddArticleToCart, AddArticleToCartHandler>
    {
        protected const int theQuantity = 3;
        protected static readonly InitiatorGuid theInitiatorGuid = new InitiatorGuid();
        protected static readonly ArticleId theArticleId = new ArticleId(12345);
        protected static readonly DateTime theFromUtc = DateTime.UtcNow;
        protected static readonly DateTime theToUtc = DateTime.UtcNow.AddDays(1);
        protected static IMemberInRole memberInRole;
        protected static Article article;
        protected static readonly OwnerId theOwnerId = new OwnerId();
        protected static ICartRepository cartRepository;

        private Establish ctx =
            () =>
            {
                article = new Article(theArticleId.Id, new Owner(theOwnerId, "Owner"), "Article", 7);
                cartRepository = depends.on<ICartRepository>();
                memberInRole = depends.on<IMemberInRole>();
                depends.on<IArticleService>().WhenToldTo(x => x.GetById(theArticleId)).Return(article);
                command = new AddArticleToCart(theInitiatorGuid, theArticleId, theFromUtc, theToUtc, theQuantity);
            };
    }

    [Subject(typeof (AddArticleToCartHandler))]
    public class when_user_has_no_cart_yet : when_add_article_to_cart_is_handled
    {
        private Establish ctx =
            () => cartRepository.WhenToldTo(x => x.FindByUserGuid(new UserGuid(theInitiatorGuid.Id))).Return((Cart)null);

        private It should_add_new_cart_to_repository = () => cartRepository.WasToldTo(x => x.Add(Arg<Cart>.Is.NotNull));

        private It should_ask_for_active_membership =
            () => memberInRole.WasToldTo(x => x.ActiveMember(theOwnerId, new UserGuid(theInitiatorGuid.Id)));
    }

    [Subject(typeof (AddArticleToCartHandler))]
    public class when_user_already_has_a_cart : when_add_article_to_cart_is_handled
    {
        private static Cart theCart;

        private Establish ctx = () =>
        {
            theCart = new Cart(theInitiatorGuid, new UserGuid(theInitiatorGuid.Id));
            cartRepository.WhenToldTo(x => x.FindByUserGuid(new UserGuid(theInitiatorGuid.Id))).Return(theCart);
        };

        private It should_ask_for_active_membership =
            () => memberInRole.WasToldTo(x => x.ActiveMember(theOwnerId, new UserGuid(theInitiatorGuid.Id)));

        private It should_not_add_a_cart_to_repository =
            () => cartRepository.WasNotToldTo(x => x.Add(Arg<Cart>.Is.Anything));
    }
}
namespace Phundus.Tests.Shop.Commands
{
    using System;
    using Common.Domain.Model;
    using Machine.Fakes;
    using Machine.Specifications;
    using Orders.Commands;
    using Phundus.Shop.Authorization;
    using Phundus.Shop.Orders.Commands;
    using Phundus.Shop.Orders.Model;
    using Phundus.Shop.Orders.Repositories;
    using Phundus.Shop.Services;
    using Rhino.Mocks;

    public class when_add_article_to_cart_is_handled :
        shop_command_handler_concern<AddArticleToCart, AddArticleToCartHandler>
    {
        protected const int theQuantity = 3;
        protected static readonly DateTime theFromUtc = DateTime.UtcNow;
        protected static readonly DateTime theToUtc = DateTime.UtcNow.AddDays(1);
        protected static Article theArticle;
        protected static readonly OwnerId theOwnerId = new OwnerId();
        protected static ICartRepository cartRepository;

        private Establish ctx = () =>
        {
            theArticle = make.Article();
            cartRepository = depends.on<ICartRepository>();

            depends.on<IArticleService>().WhenToldTo(x => x.GetById(theArticle.ArticleId, theInitiatorId)).Return(theArticle);
            command = new AddArticleToCart(theInitiatorId, theArticle.ArticleId, theFromUtc, theToUtc, theQuantity);
        };
    }

    [Subject(typeof (AddArticleToCartHandler))]
    public class when_user_has_no_cart_yet : when_add_article_to_cart_is_handled
    {
        private Establish ctx = () =>
            cartRepository.WhenToldTo(x => x.FindByUserGuid(theInitiatorId)).Return((Cart) null);

        private It should_add_new_cart_to_repository = () =>
            cartRepository.WasToldTo(x => x.Add(Arg<Cart>.Is.NotNull));

        private It should_enforce_initiator_to_rent_article = () =>
            EnforcedInitiatorTo<RentArticle>(p => Equals(p.Article.ArticleId, theArticle.ArticleId));
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

        private It should_enforce_initiator_to_rent_article = () =>
            EnforcedInitiatorTo<RentArticle>(p => Equals(p.Article.ArticleId, theArticle.ArticleId));

        private It should_not_add_a_cart_to_repository =
            () => cartRepository.WasNotToldTo(x => x.Add(Arg<Cart>.Is.Anything));
    }
}
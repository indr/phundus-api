namespace Phundus.Core.Tests.Shop.Commands
{
    using System;
    using Common.Domain.Model;
    using Core.IdentityAndAccess.Queries;
    using Core.Shop.Orders.Commands;
    using Core.Shop.Orders.Model;
    using Core.Shop.Orders.Repositories;
    using Core.Shop.Services;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;

    public class when_add_article_to_cart_is_handled : handler_concern<AddArticleToCart, AddArticleToCartHandler>
    {
        protected const int theQuantity = 3;
        protected static readonly UserId theInitiatorId = new UserId(1001);
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
                command = new AddArticleToCart(theInitiatorId, theArticleId, theFromUtc, theToUtc, theQuantity);
            };
    }

    [Subject(typeof (AddArticleToCartHandler))]
    public class when_user_has_no_cart_yet : when_add_article_to_cart_is_handled
    {
        private Establish ctx =
            () => cartRepository.WhenToldTo(x => x.FindByUserId(theInitiatorId)).Return((Cart) null);

        private It should_add_new_cart_to_repository = () => cartRepository.WasToldTo(x => x.Add(Arg<Cart>.Is.NotNull));

        private It should_ask_for_active_membership =
            () => memberInRole.WasToldTo(x => x.ActiveMember(theOwnerId, theInitiatorId));
    }

    [Subject(typeof (AddArticleToCartHandler))]
    public class when_user_already_has_a_cart : when_add_article_to_cart_is_handled
    {
        private static Cart theCart;

        private Establish ctx = () =>
        {
            theCart = new Cart(theInitiatorId);
            cartRepository.WhenToldTo(x => x.FindByUserId(theInitiatorId)).Return(theCart);
        };

        private It should_ask_for_active_membership =
            () => memberInRole.WasToldTo(x => x.ActiveMember(theOwnerId, theInitiatorId));

        private It should_not_add_a_cart_to_repository =
            () => cartRepository.WasNotToldTo(x => x.Add(Arg<Cart>.Is.Anything));
    }
}
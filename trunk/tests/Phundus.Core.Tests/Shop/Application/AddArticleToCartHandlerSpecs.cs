namespace Phundus.Tests.Shop.Application
{
    using System;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Shop.Application;
    using Phundus.Shop.Model;
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

        private static CartItemId theCartItemId;

        private Establish ctx = () =>
        {
            theArticle = make.Article();

            var lesseeId = new LesseeId(theInitiatorId);
            depends.on<ILesseeService>()
                .setup(x => x.GetById(Arg<LesseeId>.Is.Equal(lesseeId)))
                .Return(make.Lessee(lesseeId));
            cartRepository = depends.on<ICartRepository>();

            depends.on<IArticleService>()
                .WhenToldTo(
                    x =>
                        x.GetById(Arg<LessorId>.Is.Equal(theArticle.LessorId),
                            Arg<ArticleId>.Is.Equal(theArticle.ArticleId),
                            Arg<LesseeId>.Is.Equal(new LesseeId(theInitiatorId))))
                .Return(theArticle);
            theCartItemId = new CartItemId();
            command = new AddArticleToCart(theInitiatorId, theCartItemId, theArticle.LessorId, theArticle.ArticleId,
                theFromUtc, theToUtc, theQuantity);
        };
    }

    [Subject(typeof (AddArticleToCartHandler))]
    public class when_user_has_no_cart_yet : when_add_article_to_cart_is_handled
    {
        private Establish ctx = () =>
            cartRepository.WhenToldTo(x => x.FindByUserGuid(theInitiatorId)).Return((Cart) null);

        private It should_add_new_cart_to_repository = () =>
            cartRepository.WasToldTo(x => x.Add(Arg<Cart>.Is.NotNull));
    }

    [Subject(typeof (AddArticleToCartHandler))]
    public class when_user_already_has_a_cart : when_add_article_to_cart_is_handled
    {
        private static Cart theCart;

        private Establish ctx = () =>
        {
            theCart = new Cart(theInitiatorId);
            cartRepository.WhenToldTo(x => x.FindByUserGuid(theInitiatorId)).Return(theCart);
        };

        private It should_not_add_a_cart_to_repository = () =>
            cartRepository.never_received(x => x.Add(Arg<Cart>.Is.NotNull));
    }
}
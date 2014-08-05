namespace Phundus.Core.Tests.Inventory
{
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Core.Inventory.Commands;
    using Core.Inventory.Model;
    using Core.Inventory.Repositories;
    using Ddd;
    using developwithpassion.specifications.extensions;
    using developwithpassion.specifications.rhinomocks;
    using IdentityAndAccess.Queries;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;

    public abstract class concern : Observes<CreateArticleHandler>
    {
        private static IWindsorContainer container;

        protected static CreateArticle command;

        protected static IEventPublisher publisher;

        protected Establish event_publisher = () =>
        {
            publisher = depends.on<IEventPublisher>();

            container = new WindsorContainer();
            container.Register(Component.For<IEventPublisher>().Instance(publisher));
            EventPublisher.Container = container;
        };

        public Because of = () => sut.Handle(command);
    }

    [Subject(typeof (CreateArticleHandler))]
    public class when_the_command_is_handled : concern
    {
        private static IArticleRepository repository;

        private static IMemberInMembershipRoleQueries memberInMembershipRoleQueries;

        private Establish c = () =>
        {
            repository = depends.on<IArticleRepository>();
            repository.setup(x => x.GetNextIdentifier()).Return(1);

            memberInMembershipRoleQueries = depends.on<IMemberInMembershipRoleQueries>();
            memberInMembershipRoleQueries.setup(x => x.IsActiveChiefIn(1, 2)).Return(true);

            command = new CreateArticle();
            command.InitiatorId = 2;
            command.OrganizationId = 1;
        };

        public It should_add_to_repository = () => repository.WasToldTo(x => x.Add(Arg<Article>.Is.NotNull));

        public It should_ask_for_new_id = () => repository.WasToldTo(x => x.GetNextIdentifier());

        public It should_publish_article_created =
            () => publisher.WasToldTo(x => x.Publish(Arg<ArticleCreated>.Is.NotNull));

        public It should_set_article_id = () => command.ArticleId.ShouldNotBeNull();

        public It should_ask_for_chief_privileges =
            () => memberInMembershipRoleQueries.WasToldTo(x => x.IsActiveChiefIn(1, 2));
    }
}
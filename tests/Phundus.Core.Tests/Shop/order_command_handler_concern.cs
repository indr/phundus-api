namespace Phundus.Tests.Shop
{
    using Common.Commanding;
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;
    using Phundus.Shop.Model;

    public abstract class order_command_handler_concern<TCommand, THandler> :
        shop_command_handler_concern<TCommand, THandler>
        where THandler : class, IHandleCommand<TCommand> where TCommand : ICommand
    {
        protected static IOrderRepository orderRepository;
        protected static IArticleService articleService;
        protected static ILessorService lessorService;
        protected static ILesseeService lesseeService;
        protected static Manager theManager;
        protected static Lessor theLessor;
        protected static Lessee theLessee;

        private Establish ctx = () =>
        {
            orderRepository = depends.on<IOrderRepository>();
            articleService = depends.on<IArticleService>();
            lessorService = depends.on<ILessorService>();
            lesseeService = depends.on<ILesseeService>();

            theManager = make.Manager(theInitiatorId);
            theLessor = make.Lessor();
            theLessee = make.Lessee(theInitiatorId);

            userInRole.setup(x => x.Manager(theInitiatorId, theLessor.LessorId)).Return(theManager);
            lessorService.setup(x => x.GetById(theLessor.LessorId)).Return(theLessor);
            lesseeService.setup(x => x.GetById(theLessee.LesseeId)).Return(theLessee);
        };
    }
}
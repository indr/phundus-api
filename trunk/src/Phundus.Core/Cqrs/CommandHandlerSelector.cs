namespace Phundus.Core.Cqrs
{
    using System;
    using System.Reflection;
    using Castle.Facilities.TypedFactory;

    public class CommandHandlerSelector : DefaultTypedFactoryComponentSelector
    {
        public CommandHandlerSelector()
            : base(false)
        {
        }

        protected override Type GetComponentType(MethodInfo method, object[] arguments)
        {
            if (arguments.Length > 0 && arguments[0] is ICommand)
            {
                var handlerType = typeof(ICommandHandler<>).MakeGenericType(arguments[0].GetType());

                return handlerType.MakeArrayType();
            }

            return base.GetComponentType(method, arguments);
        }
    }
}
namespace Phundus.Infrastructure.Cqrs
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
                Type handlerType =
                    typeof (IHandleCommand<>).MakeGenericType(arguments[0].GetType());

                return handlerType;
            }

            return base.GetComponentType(method, arguments);
        }
    }
}
namespace Phundus.Cqrs
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

                if (method.ReturnType.IsArray)
                    return handlerType.MakeArrayType();
                return handlerType;
            }

            return base.GetComponentType(method, arguments);
        }
    }
}
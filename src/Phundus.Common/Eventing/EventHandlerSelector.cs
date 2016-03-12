namespace Phundus.Common.Eventing
{
    using System;
    using System.Reflection;
    using Castle.Facilities.TypedFactory;
    using Domain.Model;

    public class EventHandlerSelector : DefaultTypedFactoryComponentSelector
    {
        public EventHandlerSelector()
            : base(false)
        {
        }

        protected override Type GetComponentType(MethodInfo method, object[] arguments)
        {
            if (arguments.Length > 0 && arguments[0] is DomainEvent)
            {
                Type handlerType =
                    typeof (ISubscribeTo<>).MakeGenericType(arguments[0].GetType());

                if (method.ReturnType.IsArray)
                    return handlerType.MakeArrayType();
                return handlerType;
            }

            return base.GetComponentType(method, arguments);
        }

        protected override string GetComponentName(MethodInfo method, object[] arguments)
        {
            if (arguments.Length == 1 && arguments[0] is string)
            {
                return (string)arguments[0];
            }
            return base.GetComponentName(method, arguments);
        }
    }
}
namespace Phundus.Authorization
{
    using System;
    using System.Reflection;
    using Castle.Facilities.TypedFactory;

    public class AccessObjectHandlerSelector : DefaultTypedFactoryComponentSelector
    {
        public AccessObjectHandlerSelector()
            : base(false)
        {
        }

        protected override Type GetComponentType(MethodInfo method, object[] arguments)
        {
            if (arguments.Length > 0 && arguments[0] is IAccessObject)
            {
                Type handlerType =
                    typeof (IHandleAccessObject<>).MakeGenericType(arguments[0].GetType());

                if (method.ReturnType.IsArray)
                    return handlerType.MakeArrayType();
                return handlerType;
            }

            return base.GetComponentType(method, arguments);
        }
    }
}
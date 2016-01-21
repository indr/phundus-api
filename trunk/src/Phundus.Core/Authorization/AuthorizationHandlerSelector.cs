namespace Phundus.Authorization
{
    using System;
    using System.Reflection;
    using Castle.Facilities.TypedFactory;

    public class AuthorizationHandlerSelector : DefaultTypedFactoryComponentSelector
    {
        public AuthorizationHandlerSelector()
            : base(false)
        {
        }

        protected override Type GetComponentType(MethodInfo method, object[] arguments)
        {
            if (arguments.Length > 0 && arguments[0] is IAuthorization)
            {
                Type handlerType =
                    typeof (IHandleAuthorization<>).MakeGenericType(arguments[0].GetType());

                if (method.ReturnType.IsArray)
                    return handlerType.MakeArrayType();
                return handlerType;
            }

            return base.GetComponentType(method, arguments);
        }
    }
}
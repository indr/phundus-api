namespace Phundus.Authorization
{
    using System.Reflection;
    using Castle.Core;
    using Castle.DynamicProxy;
    using Castle.MicroKernel;

    [Transient]
    public class AutoReleaseAccessObjectHandlerInterceptor : IInterceptor
    {
        private readonly IKernel _kernel;

        public AutoReleaseAccessObjectHandlerInterceptor(IKernel kernel)
        {
            _kernel = kernel;
        }

        public void Intercept(IInvocation invocation)
        {
            MethodInfo methodHandle = null;
            var parameters = invocation.Method.GetParameters();
            if (parameters.Length == 2)
            {
                // TODO: Do we really need to make the generic type to compare MethodInfos?
                var genericType = typeof (IHandleAccessObject<>)
                    .MakeGenericType(parameters[1].ParameterType);
                methodHandle = genericType.GetMethod("Enforce");
                if (invocation.Method != methodHandle)
                    methodHandle = genericType.GetMethod("Test");
            }

            if (invocation.Method != methodHandle)
            {
                invocation.Proceed();
                return;
            }

            try
            {
                invocation.Proceed();
            }
            finally
            {
                _kernel.ReleaseComponent(invocation.Proxy);
            }
        }
    }
}
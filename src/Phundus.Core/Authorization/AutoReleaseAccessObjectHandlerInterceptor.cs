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
            if (parameters.Length == 1)
            {
                // TODO: Do we really need to make the generic type to compare MethodInfos?
                methodHandle = typeof (IHandleAccessObject<>)
                    .MakeGenericType(parameters[0].ParameterType)
                    .GetMethod("Handle");
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
namespace Phundus.Core.Ddd
{
    using System.Reflection;
    using Castle.Core;
    using Castle.DynamicProxy;
    using Castle.MicroKernel;
    using Common.EventPublishing;

    [Transient]
    public class AutoReleaseEventHandlerInterceptor : IInterceptor
    {
        private readonly IKernel _kernel;

        public AutoReleaseEventHandlerInterceptor(IKernel kernel)
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
                methodHandle = typeof(ISubscribeTo<>)
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
namespace Phundus.Common.Commanding
{
    using System.Reflection;
    using Castle.Core;
    using Castle.DynamicProxy;
    using Castle.MicroKernel;

    [Transient]
    public class AutoReleaseCommandHandlerInterceptor : IInterceptor
    {
        private readonly IKernel _kernel;

        public AutoReleaseCommandHandlerInterceptor(IKernel kernel)
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
                methodHandle = typeof(IHandleCommand<>)
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
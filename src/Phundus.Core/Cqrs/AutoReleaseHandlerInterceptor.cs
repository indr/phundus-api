namespace Phundus.Core.Cqrs
{
    using System.Reflection;
    using Castle.Core;
    using Castle.DynamicProxy;
    using Castle.MicroKernel;

    [Transient]
    public class AutoReleaseHandlerInterceptor : IInterceptor
    {
        private static readonly MethodInfo Execute = typeof(ICommandHandler).GetMethod("Execute");

        private readonly IKernel _kernel;


        public AutoReleaseHandlerInterceptor(IKernel kernel)
        {
            _kernel = kernel;
        }

        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method != Execute)
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
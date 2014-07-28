namespace Phundus.Core.Ddd
{
    using System.Reflection;
    using Castle.Core;
    using Castle.DynamicProxy;
    using Castle.MicroKernel;

    [Transient]
    public class AutoReleaseEventHandlerInterceptor : IInterceptor
    {
        private static readonly MethodInfo MethodHandle = typeof (ISubscribeTo<DomainEvent>).GetMethod("Handle");

        private readonly IKernel _kernel;


        public AutoReleaseEventHandlerInterceptor(IKernel kernel)
        {
            _kernel = kernel;
        }

        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method != MethodHandle)
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
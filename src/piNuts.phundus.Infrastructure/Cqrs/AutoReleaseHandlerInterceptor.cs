namespace Phundus.Infrastructure.Cqrs
{
    using System.Reflection;
    using Castle.Core;
    using Castle.DynamicProxy;
    using Castle.MicroKernel;

    [Transient]
    public class AutoReleaseHandlerInterceptor : IInterceptor
    {
        private static readonly MethodInfo MethodHandle = typeof(IHandleCommand<ICommand>).GetMethod("Handle");

        private readonly IKernel _kernel;


        public AutoReleaseHandlerInterceptor(IKernel kernel)
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
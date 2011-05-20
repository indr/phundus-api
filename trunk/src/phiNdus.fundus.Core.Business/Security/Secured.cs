using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.Security
{
    public class Secured
    {
        public static SecuredHelper With(Session session)
        {
            Rhino.Commons.Guard.Against<System.ArgumentNullException>(session == null, "session");
            return new SecuredHelper();
        }
    }

    public class SecuredHelper
    {
        public SecuredHelper()
        {
            
        }

        public void Call<ServiceType>(Proc<ServiceType> proc)
        {
            var inst = (ServiceType)System.Activator.CreateInstance(typeof(ServiceType));
            proc(inst);
        }

        public ReturnType Call<ServiceType, ReturnType>(Rhino.Commons.Func<ReturnType, ServiceType> func)
        {
            var inst = (ServiceType) System.Activator.CreateInstance(typeof (ServiceType));
            return func(inst);
        }
    }
}
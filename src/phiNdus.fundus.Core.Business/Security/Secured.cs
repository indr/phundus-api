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
            where ServiceType : new()
        {
            //var inst = (ServiceType)System.Activator.CreateInstance(typeof(ServiceType));
            //proc(inst);
            proc(new ServiceType());
        }

        public ReturnType Call<ServiceType, ReturnType>(Rhino.Commons.Func<ReturnType, ServiceType> func)
            where ServiceType : new()
        {
            //var inst = (ServiceType) System.Activator.CreateInstance(typeof (ServiceType));
            //return func(inst);
            return func(new ServiceType());
        }
    }
}
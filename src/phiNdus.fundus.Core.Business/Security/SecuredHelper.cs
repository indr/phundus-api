using System;
using phiNdus.fundus.Core.Business.Services;

namespace phiNdus.fundus.Core.Business.Security
{
    public class SecuredHelper
    {
        public SecuredHelper(Session session)
        {
            Session = session;
        }

        public void Call<TService>(Action<TService> func)
            where TService : BaseService, new()
        {
            var service = new TService();
            service.Session = Session;
            func(service);
        }

        public TResult Call<TService, TResult>(Func<TService, TResult> func)
            where TService : BaseService, new()
        {
            var service = new TService();
            service.Session = Session;
            return func(service);
        }

        protected Session Session { get; private set; }
    }
}
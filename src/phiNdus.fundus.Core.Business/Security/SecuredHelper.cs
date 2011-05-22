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

        protected Session Session { get; private set; }

        public void Call<TService>(Action<TService> func)
            where TService : BaseService, new()
        {
            var service = GetService<TService>();
            func(service);
        }

        public TResult Call<TService, TResult>(Func<TService, TResult> func)
            where TService : BaseService, new()
        {
            var service = GetService<TService>();
            return func(service);
        }

        private TService GetService<TService>() where TService : BaseService, new()
        {
            var service = new TService();
            service.Session = Session;
            return service;
        }
    }
}
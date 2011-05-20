using System;
using phiNdus.fundus.Core.Business.Services;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.Security
{
    public class Secured
    {
        public static SecuredHelper With(Session session)
        {
            Guard.Against<ArgumentNullException>(session == null, "session");
            return new SecuredHelper();
        }
    }

    public class SecuredHelper
    {
        public void Call<TService>(Action<TService> func)
            where TService : BaseService, new()
        {
            func(new TService());
        }

        public TResult Call<TService, TResult>(System.Func<TService, TResult> func)
            where TService : BaseService, new()
        {
            return func(new TService());
        }
    }
}
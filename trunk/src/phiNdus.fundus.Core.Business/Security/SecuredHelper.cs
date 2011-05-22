using System;
using phiNdus.fundus.Core.Business.Security.Constraints;
using phiNdus.fundus.Core.Business.Services;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.Security
{
    public class SecuredHelper
    {
        private AbstractConstraint _constraint;

        public SecuredHelper(AbstractConstraint constraint)
        {
            _constraint = constraint;
        }

        #region Do

        public void Do<TService>(Action<TService> func)
            where TService : BaseService, new()
        {
            func(GetService<TService>());
        }

        public TResult Do<TService, TResult>(System.Func<TService, TResult> func)
            where TService : BaseService, new()
        {
            return func(GetService<TService>());
        }

        private TService GetService<TService>() where TService : BaseService, new()
        {
            var context = new SecurityContext();
            using (UnitOfWork.Start())
            {
                if (!_constraint.Eval(context))
                    throw new AuthorizationException();
            }
            var service = new TService();
            service.SecurityContext = context;
            return service;
        }

        #endregion
    }
}
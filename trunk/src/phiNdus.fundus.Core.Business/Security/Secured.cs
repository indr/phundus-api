using System;
using phiNdus.fundus.Core.Business.Security.Constraints;
using phiNdus.fundus.Core.Business.Services;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.Security
{
    public class Secured
    {
        private readonly AbstractConstraint _constraint;

        private Secured(AbstractConstraint constraint)
        {
            _constraint = constraint;
        }

        public static Secured With(AbstractConstraint constraint)
        {
            return new Secured(constraint);
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
            SecurityContext context = null;
            if (_constraint != null)
            {
                context = new SecurityContext();
                using (UnitOfWork.Start())
                {
                    if (!_constraint.Eval(context))
                        throw new AuthorizationException(_constraint.Message);
                }
            }

            // TODO,Inder: Tjaaa... Dependency-Injection hier schwierig...
            var service = IoC.TryResolve<TService>();
            if (service == null)
                service = new TService();
            service.SecurityContext = context;
            return service;
        }

        #endregion
    }
}
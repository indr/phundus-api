using System;
using System.Diagnostics;
using phiNdus.fundus.Business.Security.Constraints;
using phiNdus.fundus.Business.Services;
using Rhino.Commons;

namespace phiNdus.fundus.Business.Security
{
    using phiNdus.fundus.Domain;
    using Rhino.Commons;
    using piNuts.phundus.Infrastructure;

    [DebuggerStepThrough]
    public class Unsecured
    {
        protected static AbstractConstraint Nothing { get { return new TrueConstraint(); } }

        public static void Do<TService>(Action<TService> func)
            where TService : BaseService, new()
        {
            Secured.With(Nothing).Do(func);
        }

        public static TResult Do<TService, TResult>(System.Func<TService, TResult> func)
            where TService : BaseService, new()
        {
            return Secured.With(Nothing).Do(func);
        }
    }

    [DebuggerStepThrough]
    public class Secured
    {
        private AbstractConstraint _constraint;

        private Secured(AbstractConstraint constraint)
        {
            _constraint = constraint;
        }

        public static Secured With(AbstractConstraint constraint)
        {
            return new Secured(constraint);
        }

        public Secured And(AbstractConstraint constraint)
        {
            _constraint &= constraint;
            return this;
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
            var service = GlobalContainer.TryResolve<TService>();
            if (service == null)
                service = new TService();
            service.SecurityContext = context;
            return service;
        }

        #endregion
    }
}
namespace Phundus.Authorization
{
    using System;
    using Common.Domain.Model;

    public interface IAuthorize
    {
        void Enforce<TAccessObject>(UserId userId, TAccessObject accessObject);
        bool Test<TAccessObject>(UserId userId, TAccessObject accessObject);
    }

    public class Authorize : IAuthorize
    {
        private readonly IAccessObjectHandlerFactory _accessObjectHandlerFactory;

        public Authorize(IAccessObjectHandlerFactory accessObjectHandlerFactory)
        {
            if (accessObjectHandlerFactory == null) throw new ArgumentNullException("accessObjectHandlerFactory");
            _accessObjectHandlerFactory = accessObjectHandlerFactory;
        }

        public void Enforce<TAccessObject>(UserId userId, TAccessObject accessObject)
        {
            var handler = GetHandler(accessObject);

            handler.Enforce(userId, accessObject);
        }

        public bool Test<TAccessObject>(UserId userId, TAccessObject accessObject)
        {
            var handler = GetHandler(accessObject);

            return handler.Test(userId, accessObject);
        }

        private IHandleAccessObject<TAccessObject> GetHandler<TAccessObject>(TAccessObject accessObject)
        {
            return _accessObjectHandlerFactory.GetHandlerForAccessObject(accessObject);
        }
    }
}
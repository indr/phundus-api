namespace Phundus.Authorization
{
    using System;
    using Common.Domain.Model;

    public interface IAuthorize
    {
        void User<TAccessObject>(UserId userId, TAccessObject accessObject);
    }

    public class Authorize : IAuthorize
    {
        private readonly IAccessObjectHandlerFactory _accessObjectHandlerFactory;

        public Authorize(IAccessObjectHandlerFactory accessObjectHandlerFactory)
        {
            if (accessObjectHandlerFactory == null) throw new ArgumentNullException("accessObjectHandlerFactory");
            _accessObjectHandlerFactory = accessObjectHandlerFactory;
        }

        public void User<TAccessObject>(UserId userId, TAccessObject accessObject)
        {
            IHandleAccessObject<TAccessObject> handler = _accessObjectHandlerFactory.GetHandlerForAccessObject(accessObject);

            handler.Handle(userId, accessObject);
        }
    }
}
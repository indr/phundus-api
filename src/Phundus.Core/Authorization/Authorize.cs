namespace Phundus.Authorization
{
    using System;
    using Common.Domain.Model;

    public class Authorize : IAuthorize
    {
        private readonly IAuthorizationHandlerFactory _authorizationHandlerFactory;

        public Authorize(IAuthorizationHandlerFactory authorizationHandlerFactory)
        {
            if (authorizationHandlerFactory == null) throw new ArgumentNullException("authorizationHandlerFactory");
            _authorizationHandlerFactory = authorizationHandlerFactory;
        }

        public void User<TAuthorization>(UserId userId, TAuthorization accessObject)
        {
            IHandleAuthorization<TAuthorization> handler = _authorizationHandlerFactory.GetHandlerForAccessObject(accessObject);

            handler.Handle(accessObject);
        }
    }
}
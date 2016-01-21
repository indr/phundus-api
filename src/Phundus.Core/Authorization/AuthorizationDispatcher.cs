namespace Phundus.Authorization
{
    public class AuthorizationDispatcher : IAuthorizationDispatcher
    {
        public IAuthorizationHandlerFactory Factory { get; set; }

        public void Dispatch<TAuthorization>(TAuthorization command)
        {
            IHandleAuthorization<TAuthorization> handler = Factory.GetHandlerForCommand(command);

            handler.Handle(command);
        }
    }
}
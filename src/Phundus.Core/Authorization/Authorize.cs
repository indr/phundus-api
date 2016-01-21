namespace Phundus.Authorization
{
    public class Authorize : IAuthorize
    {
        public IAuthorizationHandlerFactory Factory { get; set; }

        public void Dispatch<TAuthorization>(TAuthorization command)
        {
            IHandleAuthorization<TAuthorization> handler = Factory.GetHandlerForCommand(command);

            handler.Handle(command);
        }
    }
}
using System.Web;

namespace phiNdus.fundus.Core.Web.ViewModels
{
    public class ViewModelBase
    {
        protected string SessionId { get { return HttpContext.Current.Session.SessionID; } }
    }
}
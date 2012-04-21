using System.Web;

namespace phiNdus.fundus.Web.Models
{
    public abstract class ModelBase
    {
        protected string SessionId { get { return HttpContext.Current.Session.SessionID; } }
    }
}
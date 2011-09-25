using System.Web;

namespace phiNdus.fundus.Core.Web.Models
{
    public abstract class ModelBase
    {
        protected string SessionId { get { return HttpContext.Current.Session.SessionID; } }
    }
}
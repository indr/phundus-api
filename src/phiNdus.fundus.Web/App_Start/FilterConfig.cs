using System.Web.Mvc;
using phiNdus.fundus.Web.Helpers;

namespace phiNdus.fundus.Web.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new InvalidSessionKeyAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
using System;

namespace phiNdus.fundus.Web.Models.CartModels
{
    public static class SessionAdapter
    {
        public static DateTime ShopBegin
        {
            get { return DateTime.Today.AddDays(1); }
        }

        public static DateTime ShopEnd
        {
            get { return DateTime.Today.AddDays(7); }
        }
    }
}
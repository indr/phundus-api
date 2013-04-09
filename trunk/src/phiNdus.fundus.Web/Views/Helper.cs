namespace phiNdus.fundus.Web.Views
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public static class Helper
    {
        public static IHtmlString DisplayEmail(this HtmlHelper htmlHelper, string value, bool reverse = false, bool replace = false)
        {
            // http://techblog.tilllate.com/2008/07/20/ten-methods-to-obfuscate-e-mail-addresses-compared/

            if (reverse)
            {
                var charArray = value.ToCharArray();
                Array.Reverse(charArray);
                value = new String(charArray);
                value = value.Replace("@", @"<span style=""display:none;"">.null</span>@");

                if (replace)
                    value = value.Replace("@", " TA ");

                return new HtmlString(@"<span style=""direction: rtl; unicode-bidi: bidi-override;"">" + value + "</span>");
            }
            else
            {
                value = value.Replace("@", @"@<span style=""display:none;"">null.</span>");

                if (replace)
                    value = value.Replace("@", " AT ");

                return new HtmlString(value);
            }
        }

        public static IHtmlString DisplayUrl(this HtmlHelper htmlHelper, string value)
        {
            if (!value.StartsWith("http"))
                value = "http://" + value;
            return new HtmlString(value);
        }
    }
}
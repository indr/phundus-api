namespace Phundus.Common
{
    using System.Text.RegularExpressions;

    public static class FriendlyUrl
    {
        public static string ToFriendlyUrl(this string value, bool toLowerCase = true)
        {
            // http://stackoverflow.com/questions/37809/how-do-i-generate-a-friendly-url-in-c
            // http://stackoverflow.com/questions/2161684/transform-title-into-dashed-url-friendly-string

            if (value == null)
                return null;

            if (toLowerCase)
            {
                value = value.ToLowerInvariant();
            }
            value = Regex.Replace(value, " ", "-");
            return Regex.Replace(value, @"[^A-Za-zÄÖÜäöü0-9\-\._~]+", "");
        }
    }
}
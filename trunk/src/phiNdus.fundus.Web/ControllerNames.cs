namespace Phundus.Web
{
    public class ControllerNames
    {
        public static string Cart { get { return @"cart"; } }        
        public static string Home { get { return @"home"; } }
        public static string Shop { get { return @"shop"; } }
        public static string Organizations { get { return @"organizations"; } }
    }

    public class CartActionNames
    {
        public static string Index { get { return @""; } }
        public static string Add { get { return @"add"; } }
        public static string Remove { get { return @"remove"; } }
        public static string CheckOut { get { return @"checkout"; } }
    }

    public class HomeActionNames
    {
        public static string Index { get { return @""; } }
    }

    public class ShopActionNames
    {
        public static string Index { get { return @""; } }
        public static string Article { get { return @"article"; } }
        public static string AddToCart { get { return @"addtocart"; } }
    }
}
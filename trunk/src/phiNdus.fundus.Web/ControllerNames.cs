namespace Phundus.Web
{
    public class ControllerNames
    {
        public static string Account { get { return @"account";  } }
        public static string Articles { get { return @"article"; } }
        public static string Cart { get { return @"cart"; } }        
        public static string Home { get { return @"home"; } }
        public static string Orders { get { return @"order"; } }
        public static string Shop { get { return @"shop"; } }
        public static string Users { get { return @"users"; } }
        public static string Organizations { get { return @"organizations"; } }
    }
    
    public class AccountActionNames
    {
        public static string Validation { get { return @"validation"; } }
        public static string ResetPassword { get { return @"resetpassword"; } }
        public static string ChangeEmail { get { return @"changeemail"; } }
        public static string ChangePassword { get { return @"changepassword"; } }
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

    public class FeedbackActionNames
    {
        public static string Index { get { return @""; } }
    }
}
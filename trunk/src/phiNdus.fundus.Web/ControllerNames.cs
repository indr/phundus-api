namespace phiNdus.fundus.Web
{
    public class ControllerNames
    {
        public static string Account { get { return @"account";  } }
        public static string Articles { get { return @"article"; } }
        public static string Cart { get { return @"cart"; } }
        public static string Contracts { get { return @"contract"; } }
        public static string Feedback { get { return @"feedback";}}
        public static string Fields { get { return @"fields"; } }
        public static string Home { get { return @"home"; } }
        public static string Orders { get { return @"order"; } }
        public static string Settings { get { return @"settings"; } }
        public static string Shop { get { return @"shop"; } }
        public static string Users { get { return @"users"; } }
        public static string Organization { get { return @"organization"; } }
    }

    public class OrganizationActionNames
    {
        public static string Id { get { return @"id"; } }
        public static string Select { get { return @"select"; } }
        public static string Search { get { return @"search"; } }
        public static string Establish { get { return @"establish"; } }
        public static string Members { get { return @"members"; } }
        public static string Settings { get { return @"settings"; } }
    }

    public class AccountActionNames
    {
        public static string LogOn { get { return @"logon"; } }
        public static string LogOff { get { return @"logoff"; } }
        public static string Validation { get { return @"validation"; } }
        public static string Profile { get { return @"profile"; } }
        public static string ResetPassword { get { return @"resetpassword"; } }
        public static string SignUp { get { return @"signup"; } }
        public static string ChangeEmail { get { return @"changeemail"; } }
        public static string ChangePassword { get { return @"changepassword"; } }
    }

    public class ArticlesActionNames
    {
        public static string Create { get { return @"create"; } }
        public static string Edit { get { return @"edit"; } }
        public static string Fields { get { return @"fields";} }
        public static string Images { get { return @"images"; } }
        public static string ImageStore { get { return @"imagestore"; } }
        public static string Index { get { return @"Index"; } }
        public static string Availability { get { return @"availability"; } }
        public static string Categories { get { return @"categories"; } }
        public static string Reservations { get { return @"reservations"; } }
        public static string Delete { get { return @"delete"; } }
        
        // TODO: Nicht nötig, nicht wahr?
        public static string AjaxDelete { get { return @"ajaxdelete"; } }
        public static string AddPropertyAjax { get { return @"AddPropertyAjax";} }
        public static string AddDiscriminatorAjax { get { return @"AddDiscriminatorAjax"; } }
        public static string AddChild { get { return @"AddChild"; } }
    }

    public class CartActionNames
    {
        public static string Index { get { return @""; } }
        public static string Add { get { return @"add"; } }
        public static string Remove { get { return @"remove"; } }
        public static string CheckOut { get { return @"checkout"; } }
    }

    public class ContractsActionNames
    {
        public static string Index { get { return @"index"; } }
        public static string Signed { get { return @"signed"; } }
        public static string Closed { get { return @"closed"; } }
    }

    public class FieldsActionNames
    {
        public static string Create { get { return @"create"; } }
        public static string Delete { get { return @"delete"; } }
        public static string Edit { get { return @"edit"; } }
        public static string Index { get { return @""; } }
    }

    public class HomeActionNames
    {
        public static string Index { get { return @""; } }
    }

    public class OrdersActionNames
    {
        public static string Approved { get { return @"approved"; } }
        public static string Closed { get { return @"closed"; } }
        public static string Confirm { get { return @"confirm"; } }
        public static string Details { get { return @"details"; } }
        public static string Index { get { return @""; } }
        public static string Pending { get { return @"pending"; } }
        public static string Print { get { return @"print"; } }
        public static string Reject { get { return @"reject"; } }
        public static string Rejected { get { return @"rejected"; } }
    }

    public class ShopActionNames
    {
        public static string Index { get { return @""; } }
        public static string Article { get { return @"article"; } }
        public static string AddToCart { get { return @"addtocart"; } }
    }

    public class SettingsActionNames
    {
        public static string General { get { return @"general"; } }
        public static string Index { get { return @""; } }
        public static string MailGeneral { get { return @"mailgeneral"; } }
        public static string MailTemplates { get { return @"mailtemplates"; } }
        public static string SendTestEmail { get { return @"sendtestemail"; } }
    }

    public class UsersActionNames
    {
        public static string Index { get { return @""; } }
        public static string Edit { get { return @"edit"; } }
        public static string LockOut { get { return @"lockout"; } }
        public static string Unlock { get { return @"unlock"; } }
    }

    public class FeedbackActionNames
    {
        public static string Index { get { return @""; } }
    }
}
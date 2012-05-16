namespace phiNdus.fundus.Web
{
    public class ControllerNames
    {
        public static string Account { get { return @"account";  } }
        public static string Articles { get { return @"article"; } }
        public static string Cart { get { return @"cart"; } }
        public static string Contracts { get { return @"contract"; } }
        public static string Fields { get { return @"fields"; } }
        public static string Home { get { return @"home"; } }
        public static string Orders { get { return @"order"; } }
        public static string Settings { get { return @"settings"; } }
        public static string Shop { get { return @"shop"; } }
        public static string Users { get { return @"user"; } }
    }

    public class SettingsActionNames
    {
        public static string General { get { return @"general"; } }
        public static string MailGeneral { get { return @"mailgeneral"; } }
        public static string MailTemplates { get { return @"mailtemplates";} }

        public static string SendTestEmail { get { return @"sendtestemail"; } }
    }

    public class OrdersActionNames
    {
        public static string Confirm { get { return @"confirm"; } }
        public static string Reject { get { return @"reject"; } }
        public static string Print { get { return @"print"; } }
    }

    public class UsersActionNames
    {
        public static string LockOut { get { return @"lockout"; } }
        public static string Unlock { get { return @"unlock"; } }
    }
}
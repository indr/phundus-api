namespace phiNdus.fundus.Web
{
    public class ControllerNames
    {
        public static string Settings { get { return @"settings"; } }
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
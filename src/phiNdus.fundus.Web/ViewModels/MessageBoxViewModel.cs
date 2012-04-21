namespace phiNdus.fundus.Web.ViewModels
{
    public enum MessageBoxType
    {
        Error,
        Warning,
        Info,
        Success
    }

    public class MessageBox
    {
        public static MessageBoxViewModel Success(string message)
        {
            return new MessageBoxViewModel
            {
                Message = message,
                Type = MessageBoxType.Success
            };
        }

        public static MessageBoxViewModel Error(string message)
        {
            return new MessageBoxViewModel
                       {
                           Message = message,
                           Type = MessageBoxType.Error
                       };
        }
    }

    public class MessageBoxViewModel
    {
        public string Message { get; set; }
        public MessageBoxType Type { get; set; }

        
    }
}
namespace phiNdus.fundus.Domain.Infrastructure
{
    using System.Configuration;

    public static class Config
    {
        public static readonly string ServerUrl;
        public static readonly string FeedbackRecipients;

        static Config()
        {
            ServerUrl = ConfigurationManager.AppSettings["ServerUrl"];
            FeedbackRecipients = ConfigurationManager.AppSettings["FeedbackRecipients"];
        }
    }
}
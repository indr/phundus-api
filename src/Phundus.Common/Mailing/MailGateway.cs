namespace Phundus.Common.Mailing
{
    using System;
    using System.Net.Mail;
    using System.Net.Mime;
    using System.Text.RegularExpressions;
    using System.Web.Hosting;
    using Castle.Core.Logging;

    public class MailGateway : IMailGateway
    {
        public ILogger Logger { get; set; }

        public void Send(DateTime date, string recipients, string subject, string body)
        {
            if (recipients == null) throw new ArgumentNullException("recipients");

            var message = new MailMessage();
            message.To.Add(recipients);
            message.Subject = subject;
            message.Body = body;
            Send(date, message);
        }

        public void Send(DateTime date, MailMessage message)
        {
            if (date < DateTimeProvider.UtcNow.AddDays(-1))
            {
                Logger.Warn("Skipped sending mail message because it is too old.");
                return;
            }

            if (Config.InMaintenance)
            {
                RemoveUnallowedRecipients(message);
                if (message.To.Count == 0)
                    return;
            }
            SendUsingSystemNetMailSettings(message);
            SaveToPickupDirectory(message);
        }

        private void RemoveUnallowedRecipients(MailMessage message)
        {
            for (var idx = message.To.Count - 1; idx >= 0; idx--)
            {
                var to = message.To[idx];
                if (!Regex.Match(to.Address, @"@(test\.)?phundus\.ch$", RegexOptions.IgnoreCase).Success)
                    message.To.Remove(to);
            }
        }

        private static void SendUsingSystemNetMailSettings(MailMessage message)
        {
            var client = GetClient();
            client.Send(message);
        }

        private static void SaveToPickupDirectory(MailMessage message)
        {
            var defaultPickupDirectoryLocation = MapPath(@"~\App_Data\Mails");

            // If the system.net.mail.settings is equal to our default pickup directory location, prevent sending / storing
            if (GetClient().PickupDirectoryLocation == defaultPickupDirectoryLocation)
                return;

            var client = new SmtpClient("", 0);
            client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
            client.PickupDirectoryLocation = defaultPickupDirectoryLocation;
            // Pickup delivery method requires EnableSsl to be false
            client.EnableSsl = false;
            client.Send(message);
        }

        private static SmtpClient GetClient()
        {
            var client = new SmtpClient();
            if (String.IsNullOrWhiteSpace(client.PickupDirectoryLocation))
                return client;

            if (!System.IO.Path.IsPathRooted(client.PickupDirectoryLocation))
            {
                var path = MapPath(client.PickupDirectoryLocation);
                client.PickupDirectoryLocation = path;
            }
            return client;
        }

        private static string MapPath(string path)
        {
            return HostingEnvironment.MapPath(path);
        }
    }
}
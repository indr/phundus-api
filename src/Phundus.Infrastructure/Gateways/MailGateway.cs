namespace Phundus.Infrastructure.Gateways
{
    using System;
    using System.Net.Mail;
    using System.Text.RegularExpressions;
    using System.Web;

    public class MailGateway : IMailGateway
    {
        public void Send(string recipients, string subject, string body)
        {
            if (recipients == null) throw new ArgumentNullException("recipients");

            var message = new MailMessage();
            message.To.Add(recipients);
            message.Subject = subject;
            message.Body = body;
            Send(message);
        }

        public void Send(MailMessage message)
        {
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
            return HttpContext.Current.Server.MapPath(path);
        }
    }
}
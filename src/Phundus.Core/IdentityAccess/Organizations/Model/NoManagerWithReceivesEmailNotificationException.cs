namespace Phundus.IdentityAccess.Organizations.Model
{
    using System;

    public class NoManagerWithReceivesEmailNotificationException : ApplicationException
    {
        public NoManagerWithReceivesEmailNotificationException() : base("Mindestens ein Manager muss E-Mail-Benachrichtigungen erhalten.")
        {
            
        }
    }
}
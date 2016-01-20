namespace Phundus.Rest.Auth
{
    using System;

    public class MaintenanceModeException : Exception
    {
        public MaintenanceModeException(string message) : base(message)
        {
            
        }
    }
}
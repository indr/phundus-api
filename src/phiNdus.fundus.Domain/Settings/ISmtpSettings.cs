namespace phiNdus.fundus.Core.Domain.Settings
{
    public interface ISmtpSettings
    {
        string Host { get; }
        string From { get; }
        string UserName { get; }
        string Password { get; }
    }
}
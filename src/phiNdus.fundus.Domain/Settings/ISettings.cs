namespace phiNdus.fundus.Domain.Settings
{
    public interface ISettings
    {
        IMailSettings Mail { get; }
        ICommonSettings Common { get; }
    }
}
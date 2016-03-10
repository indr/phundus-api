namespace Phundus.Common.Domain.Model
{
    public interface IShortIdGeneratorService
    {
        TShortId GetNext<TShortId>() where TShortId : Identity<int>;
    }
}
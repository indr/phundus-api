namespace Phundus.Common.Messaging
{
    public interface IBus
    {
        void Send<T>(T message);
    }
}
namespace Phundus.Common.Mailing
{
    public abstract class MailModel
    {
        protected MailModel()
        {
            Urls = new Urls();
        }

        public Urls Urls { get; private set; }
    }
}
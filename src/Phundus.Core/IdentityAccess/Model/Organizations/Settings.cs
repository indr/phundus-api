namespace Phundus.IdentityAccess.Organizations.Model
{
    using System.Collections.Generic;
    using Common.Domain.Model;

    public class Settings : ValueObject
    {
        private bool _publicRental = true;
        private string _pdfTemplateFileName;
        private string _orderReceivedText;

        public Settings()
        {
        }

        public Settings(bool publicRental, string pdfTemplateFileName, string orderReceivedText)
        {
            _publicRental = publicRental;
            _pdfTemplateFileName = pdfTemplateFileName;
            _orderReceivedText = orderReceivedText;
        }

        public virtual bool PublicRental
        {
            get { return _publicRental; }
            protected set { _publicRental = value; }
        }

        public virtual string PdfTemplateFileName
        {
            get { return _pdfTemplateFileName; }
            protected set { _pdfTemplateFileName = value; }
        }

        public virtual string OrderReceivedText
        {
            get { return _orderReceivedText; }
            private set { _orderReceivedText = value; }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return PublicRental;
            yield return PdfTemplateFileName;
            yield return OrderReceivedText;
        }
    }
}
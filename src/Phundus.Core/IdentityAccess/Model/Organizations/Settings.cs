namespace Phundus.IdentityAccess.Organizations.Model
{
    using System.Collections.Generic;
    using Common.Domain.Model;

    public class Settings : ValueObject
    {
        private bool _publicRental = true;
        private string _pdfTemplateFileName;

        public Settings()
        {
        }

        public Settings(bool publicRental, string pdfTemplateFileName)
        {
            _publicRental = publicRental;
            _pdfTemplateFileName = pdfTemplateFileName;
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

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return PublicRental;
            yield return PdfTemplateFileName;
        }
    }
}
namespace Phundus.IdentityAccess.Organizations.Model
{
    using System.Collections.Generic;
    using Common.Domain.Model;

    public class Settings : ValueObject
    {
        private bool _publicRental;

        public Settings(bool publicRental = true)
        {
            _publicRental = publicRental;
        }

        protected Settings()
        {
        }

        public virtual bool PublicRental
        {
            get { return _publicRental; }
            protected set { _publicRental = value; }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return PublicRental;
        }
    }
}
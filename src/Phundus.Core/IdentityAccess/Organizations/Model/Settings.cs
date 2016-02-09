namespace Phundus.IdentityAccess.Organizations.Model
{
    using System.Collections.Generic;
    using Common.Domain.Model;

    public class Settings : ValueObject
    {
        private bool _publicRental = true;

        public Settings()
        {
        }

        public Settings(bool publicRental)
        {
            _publicRental = publicRental;
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
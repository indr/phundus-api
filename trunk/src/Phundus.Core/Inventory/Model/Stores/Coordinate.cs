namespace Phundus.Inventory.Model.Stores
{
    using System.Collections.Generic;
    using Common;
    using Common.Domain.Model;

    public class Coordinate : ValueObject
    {
        private decimal _latitude;
        private decimal _longitude;

        public Coordinate(decimal latitude, decimal longitude)
        {
            AssertionConcern.AssertArgumentRange(latitude, -90, 90, "Latitude must be between -90 and 90 degrees.");
            AssertionConcern.AssertArgumentRange(longitude, -180, 180, "Longitude must be between -180 and 180 degrees.");

            _latitude = latitude;
            _longitude = longitude;
        }

        protected Coordinate()
        {
        }

        public virtual decimal Latitude
        {
            get { return _latitude; }
            protected set { _latitude = value; }
        }

        public virtual decimal Longitude
        {
            get { return _longitude; }
            protected set { _longitude = value; }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Latitude;
            yield return Longitude;
        }

        protected bool Equals(Coordinate other)
        {
            return base.Equals(other) && _latitude == other._latitude && _longitude == other._longitude;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Coordinate) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = (hashCode*397) ^ _latitude.GetHashCode();
                hashCode = (hashCode*397) ^ _longitude.GetHashCode();
                return hashCode;
            }
        }
    }
}
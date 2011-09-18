using System;
using Iesi.Collections.Generic;

namespace phiNdus.fundus.Core.Domain.Entities
{
    public class Article : DomainObject
    {
        public Article()
        {
        }

        public Article(ISet<DomainPropertyValue> propertyValues) : base(propertyValues)
        {
        }

        public bool IsReservable
        {
            get
            {
                if (!HasProperty(DomainPropertyDefinition.ReservierbarId))
                    return false;
                return Convert.ToBoolean(GetPropertyValue(DomainPropertyDefinition.ReservierbarId));
            }
            set { throw new NotImplementedException(); }
        }

        public bool IsLendable
        {
            get
            {
                if (!HasProperty(DomainPropertyDefinition.AusleihbarId))
                    return false;
                return Convert.ToBoolean(GetPropertyValue(DomainPropertyDefinition.AusleihbarId));
            }
            set { throw new NotImplementedException(); }
        }

        public int Amount
        {
            get
            {
                if (!HasProperty(DomainPropertyDefinition.MengeId))
                    return 0;
                return Convert.ToInt32(GetPropertyValue(DomainPropertyDefinition.MengeId));
            }
            set { throw new NotImplementedException(); }
        }

        public double Price
        {
            get
            {
                if (!HasProperty(DomainPropertyDefinition.PreisId))
                    return 0.0d;
                return Convert.ToDouble(GetPropertyValue(DomainPropertyDefinition.PreisId));
            }
            set { throw new NotImplementedException(); }
        }
    }
}
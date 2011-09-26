using System;
using Iesi.Collections.Generic;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;

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

        public Article(int id, int version) : base(id, version)
        {
        }

        public virtual bool IsReservable
        {
            get
            {
                if (!HasProperty(DomainPropertyDefinition.IsReservableId))
                    return false;
                return Convert.ToBoolean(GetPropertyValue(DomainPropertyDefinition.IsReservableId));
            }
            set
            {
                if (!HasProperty(DomainPropertyDefinition.IsReservableId))
                    AddProperty(IoC.Resolve<IDomainPropertyDefinitionRepository>().Get(DomainPropertyDefinition.IsReservableId));
                SetPropertyValue(DomainPropertyDefinition.IsReservableId, value);
            }
        }

        public virtual bool IsBorrowable
        {
            get
            {
                if (!HasProperty(DomainPropertyDefinition.IsBorrowableId))
                    return false;
                return Convert.ToBoolean(GetPropertyValue(DomainPropertyDefinition.IsBorrowableId));
            }
            set
            {
                if (!HasProperty(DomainPropertyDefinition.IsBorrowableId))
                    AddProperty(IoC.Resolve<IDomainPropertyDefinitionRepository>().Get(DomainPropertyDefinition.IsBorrowableId));
                SetPropertyValue(DomainPropertyDefinition.IsBorrowableId, value);
            }
        }

        public virtual int Stock
        {
            get
            {
                if (!HasProperty(DomainPropertyDefinition.StockId))
                    return 0;
                return Convert.ToInt32(GetPropertyValue(DomainPropertyDefinition.StockId));
            }
            set
            {
                if (!HasProperty(DomainPropertyDefinition.StockId))
                    AddProperty(IoC.Resolve<IDomainPropertyDefinitionRepository>().Get(DomainPropertyDefinition.StockId));
                SetPropertyValue(DomainPropertyDefinition.StockId, value);
            }
        }

        public virtual double Price
        {
            get
            {
                if (!HasProperty(DomainPropertyDefinition.PriceId))
                    return 0.0d;
                return Convert.ToDouble(GetPropertyValue(DomainPropertyDefinition.PriceId));
            }
            set
            {
                if (!HasProperty(DomainPropertyDefinition.PriceId))
                    AddProperty(IoC.Resolve<IDomainPropertyDefinitionRepository>().Get(DomainPropertyDefinition.PriceId));
                SetPropertyValue(DomainPropertyDefinition.PriceId, value);
            }
        }
    }
}
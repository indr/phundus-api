using System;
using System.Linq;
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

        public Article(ISet<DomainPropertyValue> propertyValues)
            : base(propertyValues)
        {
        }

        public Article(int id, int version)
            : base(id, version)
        {
        }

        public virtual bool IsReservable
        {
            get
            {
                return HasProperty(FieldDefinition.IsReservableId) &&
                       Convert.ToBoolean(GetPropertyValue(FieldDefinition.IsReservableId));
            }
            set
            {
                if (!HasProperty(FieldDefinition.IsReservableId))
                    AddProperty(
                        IoC.Resolve<IDomainPropertyDefinitionRepository>().Get(FieldDefinition.IsReservableId));
                SetPropertyValue(FieldDefinition.IsReservableId, value);
            }
        }

        public virtual bool IsBorrowable
        {
            get
            {
                return HasProperty(FieldDefinition.IsBorrowableId) &&
                       Convert.ToBoolean(GetPropertyValue(FieldDefinition.IsBorrowableId));
            }
            set
            {
                if (!HasProperty(FieldDefinition.IsBorrowableId))
                    AddProperty(
                        IoC.Resolve<IDomainPropertyDefinitionRepository>().Get(FieldDefinition.IsBorrowableId));
                SetPropertyValue(FieldDefinition.IsBorrowableId, value);
            }
        }

        /// <summary>
        /// Menge, verwende in Zukunft GrossStock (Bruttobestand)
        /// </summary>
        ///[Obsolete("Siehe GrossStock")]
        public virtual int Stock
        {
            get
            {
                return !HasProperty(FieldDefinition.StockId)
                           ? 0
                           : Convert.ToInt32(GetPropertyValue(FieldDefinition.StockId));
            }
            set
            {
                if (!HasProperty(FieldDefinition.StockId))
                    AddProperty(IoC.Resolve<IDomainPropertyDefinitionRepository>().Get(FieldDefinition.StockId));
                SetPropertyValue(FieldDefinition.StockId, value);
            }
        }

        /// <summary>
        /// Preis, quasi inkl. MWSt, da alles ohne Mehrwertsteuer ;o)
        /// Grund: Wird einmal MWSt eingeführt, so sind die Preise keine
        /// ungeraden Zahlen.
        /// </summary>
        public virtual double Price
        {
            get
            {
                return !HasProperty(FieldDefinition.PriceId)
                           ? 0.0d
                           : Convert.ToDouble(GetPropertyValue(FieldDefinition.PriceId));
            }
            set
            {
                if (!HasProperty(FieldDefinition.PriceId))
                    AddProperty(IoC.Resolve<IDomainPropertyDefinitionRepository>().Get(FieldDefinition.PriceId));
                SetPropertyValue(FieldDefinition.PriceId, value);
            }
        }

        /// <summary>
        /// Bestand (Brutto)
        /// </summary>
        public virtual int GrossStock
        {
            get
            {
                if (HasProperty(FieldDefinition.GrossStockId))
                {
                    if (HasChildren)
                        throw new IllegalAttachedFieldException();
                    return Convert.ToInt32(GetPropertyValue(FieldDefinition.GrossStockId));
                }
                return !HasChildren ? 1 : Children.Sum(child => ((Article) child).GrossStock);
            }
            set
            {
                if (HasChildren)
                    throw new InvalidOperationException(
                        "Der Bruttobestand kann nicht gesetzt werden, da mindestens eine Ausprägung vorhanden ist.");
                if (!HasProperty(FieldDefinition.GrossStockId))
                    AddProperty(
                        IoC.Resolve<IDomainPropertyDefinitionRepository>().Get(FieldDefinition.GrossStockId));
                SetPropertyValue(FieldDefinition.GrossStockId, value);
            }
        }

        public virtual int BorrowableStock
        {
            get
            {
                return GrossStock - OrderRepository.SumReservedAmount(Id)
                    + Children.Sum(child => ((Article) child).BorrowableStock);
            }
        }

        protected virtual IOrderRepository OrderRepository
        {
            get { return IoC.Resolve<IOrderRepository>(); }
        }
    }
}
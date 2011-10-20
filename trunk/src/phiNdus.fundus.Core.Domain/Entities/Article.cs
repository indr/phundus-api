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
                return HasProperty(DomainPropertyDefinition.IsReservableId) &&
                       Convert.ToBoolean(GetPropertyValue(DomainPropertyDefinition.IsReservableId));
            }
            set
            {
                if (!HasProperty(DomainPropertyDefinition.IsReservableId))
                    AddProperty(
                        IoC.Resolve<IDomainPropertyDefinitionRepository>().Get(DomainPropertyDefinition.IsReservableId));
                SetPropertyValue(DomainPropertyDefinition.IsReservableId, value);
            }
        }

        public virtual bool IsBorrowable
        {
            get
            {
                return HasProperty(DomainPropertyDefinition.IsBorrowableId) &&
                       Convert.ToBoolean(GetPropertyValue(DomainPropertyDefinition.IsBorrowableId));
            }
            set
            {
                if (!HasProperty(DomainPropertyDefinition.IsBorrowableId))
                    AddProperty(
                        IoC.Resolve<IDomainPropertyDefinitionRepository>().Get(DomainPropertyDefinition.IsBorrowableId));
                SetPropertyValue(DomainPropertyDefinition.IsBorrowableId, value);
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
                return !HasProperty(DomainPropertyDefinition.StockId)
                           ? 0
                           : Convert.ToInt32(GetPropertyValue(DomainPropertyDefinition.StockId));
            }
            set
            {
                if (!HasProperty(DomainPropertyDefinition.StockId))
                    AddProperty(IoC.Resolve<IDomainPropertyDefinitionRepository>().Get(DomainPropertyDefinition.StockId));
                SetPropertyValue(DomainPropertyDefinition.StockId, value);
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
                return !HasProperty(DomainPropertyDefinition.PriceId)
                           ? 0.0d
                           : Convert.ToDouble(GetPropertyValue(DomainPropertyDefinition.PriceId));
            }
            set
            {
                if (!HasProperty(DomainPropertyDefinition.PriceId))
                    AddProperty(IoC.Resolve<IDomainPropertyDefinitionRepository>().Get(DomainPropertyDefinition.PriceId));
                SetPropertyValue(DomainPropertyDefinition.PriceId, value);
            }
        }

        /// <summary>
        /// Bestand (Brutto)
        /// </summary>
        public virtual int GrossStock
        {
            get
            {
                if (HasProperty(DomainPropertyDefinition.GrossStockId))
                {
                    if (HasChildren)
                        throw new IllegalAttachedFieldException();
                    return Convert.ToInt32(GetPropertyValue(DomainPropertyDefinition.GrossStockId));
                }
                return !HasChildren ? 1 : Children.Sum(child => ((Article) child).GrossStock);
            }
            set
            {
                if (HasChildren)
                    throw new InvalidOperationException(
                        "Der Bruttobestand kann nicht gesetzt werden, da mindestens eine Ausprägung vorhanden ist.");
                if (!HasProperty(DomainPropertyDefinition.GrossStockId))
                    AddProperty(
                        IoC.Resolve<IDomainPropertyDefinitionRepository>().Get(DomainPropertyDefinition.GrossStockId));
                SetPropertyValue(DomainPropertyDefinition.GrossStockId, value);
            }
        }

        public virtual int BorrowableStock
        {
            get
            {
                return
                    GrossStock - OrderRepository.SumReservedAmount(Id);
            }
        }

        protected virtual IOrderRepository OrderRepository
        {
            get { return IoC.Resolve<IOrderRepository>(); }
        }
    }
}
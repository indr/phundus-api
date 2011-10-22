using System;
using System.Linq;
using Iesi.Collections.Generic;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.Entities
{
    public class Article : CompositeEntity
    {
        public Article()
        {
            _createDate = DateTime.Now;
        }

        public Article(ISet<FieldValue> fieldValues)
            : base(fieldValues)
        {
            _createDate = DateTime.Now;
        }

        public Article(int id, int version)
            : base(id, version)
        {
            _createDate = DateTime.Now;
        }
        
        protected virtual IOrderRepository OrderRepository
        {
            get { return IoC.Resolve<IOrderRepository>(); }
        }

        private DateTime _createDate;
        public virtual DateTime CreateDate
        {
            get { return _createDate; }
            protected set { _createDate = value; }
        }

        public virtual bool IsReservable
        {
            get { return GetFieldValueAsBoolean(FieldDefinition.IsReservableId, false); }
            set { SetFieldValue(FieldDefinition.IsReservableId, value, true); }
        }

        public virtual bool IsBorrowable
        {
            get { return GetFieldValueAsBoolean(FieldDefinition.IsBorrowableId, false); }
            set { SetFieldValue(FieldDefinition.IsBorrowableId, value, true); }
        }

        public virtual string Caption
        {
            get { return GetFieldValueAsString(FieldDefinition.CaptionId, ""); }
            set { SetFieldValue(FieldDefinition.CaptionId, value, true); }
        }


        /// <summary>
        /// Menge, verwende in Zukunft GrossStock (Bruttobestand)
        /// </summary>
        ///[Obsolete("Siehe GrossStock")]
        public virtual int Stock
        {
            get { return GetFieldValueAsInt32(FieldDefinition.StockId, 0); }
            set { SetFieldValue(FieldDefinition.StockId, value, true); }
        }

        /// <summary>
        /// Preis, quasi inkl. MWSt, da alles ohne Mehrwertsteuer ;o)
        /// Grund: Wird einmal MWSt eingeführt, so sind die Preise keine
        /// ungeraden Zahlen.
        /// </summary>
        public virtual double Price
        {
            get { return GetFieldValueAsDouble(FieldDefinition.PriceId, 0.0d); }
            set { SetFieldValue(FieldDefinition.PriceId, value, true); }
        }

        /// <summary>
        /// Bestand (Brutto)
        /// </summary>
        public virtual int GrossStock
        {
            get
            {
                if (HasField(FieldDefinition.GrossStockId))
                {
                    if (HasChildren)
                        throw new IllegalAttachedFieldException();
                    return Convert.ToInt32(GetFieldValue(FieldDefinition.GrossStockId));
                }
                return !HasChildren ? 1 : Children.Sum(child => ((Article) child).GrossStock);
            }
            set
            {
                if (HasChildren)
                    throw new InvalidOperationException(
                        "Der Bruttobestand kann nicht gesetzt werden, da mindestens eine Ausprägung vorhanden ist.");
                if (!HasField(FieldDefinition.GrossStockId))
                    AddField(
                        IoC.Resolve<IFieldDefinitionRepository>().Get(FieldDefinition.GrossStockId));
                SetFieldValue(FieldDefinition.GrossStockId, value);
            }
        }

        /// <summary>
        /// Reservierbarer Bestand
        /// </summary>
        public virtual int ReservableStock
        {
            get
            {
                return GrossStock - OrderRepository.SumReservedAmount(Id)
                       + Children.Sum(child => ((Article) child).ReservableStock);
            }
        }
    }
}
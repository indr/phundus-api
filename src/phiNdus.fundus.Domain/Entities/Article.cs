using System;
using System.Linq;
using Iesi.Collections.Generic;
using phiNdus.fundus.Domain.Repositories;

namespace phiNdus.fundus.Domain.Entities
{
    using Microsoft.Practices.ServiceLocation;
    using piNuts.phundus.Infrastructure;

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
            get { return ServiceLocator.Current.GetInstance<IOrderRepository>(); }
        }

        public virtual Organization Organization { get; set; }

        private ISet<Image> _images = new HashedSet<Image>();
        public virtual ISet<Image> Images
        {
            get { return _images; }
            set { _images = value; }
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
                        ServiceLocator.Current.GetInstance<IFieldDefinitionRepository>().ById(FieldDefinition.GrossStockId));
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

        public virtual bool AddImage(Image image)
        {
            var result = Images.Add(image);
            image.Article = this;
            return result;
        }

        public virtual bool RemoveImage(Image image)
        {
            var result = Images.Remove(image);
            image.Article = null;
            return result;
        }
    }
}
using System;
using System.Linq;

namespace phiNdus.fundus.Core.Domain.Entities
{
    public class OrderItem : Entity
    {
        public virtual Order Order { get; set; }

        private int _amount;

        public virtual int Amount
        {
            get { return _amount; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value", "Die Menge darf nicht kleiner als Eins sein.");
                _amount = value;
            }
        }

        public virtual DateTime From { get; set; }

        public virtual DateTime To { get; set; }

        public virtual Article Article { get; set; }
    }
}
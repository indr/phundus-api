using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace phiNdus.fundus.Core.Domain.Entities
{
    public class ContractItem : BaseEntity
    {
        public ContractItem() : this(0, 0)
        {
        }

        public ContractItem(int id, int version) : base(id, version)
        {
            
        }

        public DateTime? ReturnDate { get; set; }

        public Contract Contract { get; set; }

        public int Amount { get; set; }
        public Article Article { get; set; }
        public string InventoryCode { get; set; }
        public string Name { get; set; }
        
    }
}

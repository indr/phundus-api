using System;

namespace phiNdus.fundus.Core.Web.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string ReserverName { get; set; }
        public int ItemCount { get; set; }

        public DateTime ModifyDate { get; set; }
        public string ModifierName { get; set; }
    }
}
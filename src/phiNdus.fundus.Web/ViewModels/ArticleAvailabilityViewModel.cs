using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace phiNdus.fundus.Web.ViewModels
{
    using Phundus.Core.Inventory.Queries;

    public class ArticleAvailabilityViewModel
    {
        public int Id { get; set; }
        public IList<AvailabilityDto> Availabilites { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using phiNdus.fundus.Business.Dto;

namespace phiNdus.fundus.Web.ViewModels
{
    public class ArticleAvailabilityViewModel
    {
        public int Id { get; set; }
        public IList<AvailabilityDto> Availabilites { get; set; }
    }
}
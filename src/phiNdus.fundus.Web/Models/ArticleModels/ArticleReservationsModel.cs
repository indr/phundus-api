using System.Collections.Generic;
using phiNdus.fundus.Domain.Entities;

namespace phiNdus.fundus.Web.Models.ArticleModels
{
    public class ArticleReservationsModel
    {
        public ICollection<Reservation> Items { get; set; }
    }
}
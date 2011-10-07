using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.Paging {

    /// <summary>
    /// Liefert ein "gepagtes" Resultat für einen bestimmten Typ.
    /// </summary>
    public class PagedResult<T> {

        /// <summary>
        /// Index des Seitenausschnittes in Items.
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Anzahl Elemente pro Seite.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Die Anzahl Total vorhandenen Elemente.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Die Items für die angegebene Konfiguration.
        /// </summary>
        public IList<T> Items { get; set; }

        public static PagedResult<T> For<T>(PageIndex pageIndex, IList<T> items, int total) {
            Guard.Against<ArgumentNullException>(pageIndex == null, "pageIndex");
            Guard.Against<ArgumentNullException>(items == null, "items");

            return new PagedResult<T> {
                PageIndex = pageIndex.Index,
                PageSize = pageIndex.PageSize,
                Items = items,
                Total = total
            };
        }
    }
}

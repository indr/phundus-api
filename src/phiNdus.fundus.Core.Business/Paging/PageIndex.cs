using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Business.Paging {

    public enum OrderKind {
        Asc,
        Desc
    }

    /// <summary>
    /// Definiert den zu ladenden Ausschnitt für einen bestimmten Typ.
    /// </summary>
    public class PageIndex {

        /// <summary>
        /// Index des Seitenausschnittes.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Anzahl Elemente pro Seite.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Feld nach welchem Sortiert werden soll.
        /// </summary>
        public string OrderField { get; set; }

        /// <summary>
        /// Sortierrichtung, die verwendet werden soll.
        /// </summary>
        public OrderKind OrderKind { get; set; }
    }
}

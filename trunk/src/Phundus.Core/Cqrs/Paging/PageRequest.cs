namespace Phundus.Core.Cqrs.Paging
{
    /// <summary>
    /// Definiert den zu ladenden Ausschnitt.
    /// </summary>
    public class PageRequest
    {
        /// <summary>
        /// Nullbasierte Index des Seitenausschnittes.
        /// </summary>
        public int Index { get; set; }

        private int _size = 8;

        /// <summary>
        /// Anzahl Elemente pro Seite.
        /// </summary>
        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }
    }
}
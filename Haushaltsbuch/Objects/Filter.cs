namespace Haushaltsbuch.Objects
{
    /// <summary>
    /// Klasse, die einen Filter repräsentiert.
    /// </summary>
    public class Filter
    {
        /// <summary>
        /// Holt oder setzt Kategorie als Filterkriterium.
        /// </summary>
        public string Category
        {
            get;
            set;
        }

        /// <summary>
        /// Holt oder setzt Monat als Filterkriterium.
        /// </summary>
        public string Month
        {
            get;
            set;
        }

        /// <summary>
        /// Holt oder setzt Suchbegriff als Filterkriterium.
        /// </summary>
        public string SearchTerm
        {
            get;
            set;
        }

        /// <summary>
        /// Holt oder setzt Jahr als Filterkriterium.
        /// </summary>
        public string Year
        {
            get;
            set;
        }
    }
}
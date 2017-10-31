namespace Haushaltsbuch.Objects
{
    /// <summary>
    /// Klasse, die Diagrammdatum repräsentiert.
    /// </summary>
    public class ChartData
    {
        /// <summary>
        /// Holt oder setzt Name der Gruppe in Diagramm.
        /// </summary>
        public string Group
        {
            get;
            set;
        }

        /// <summary>
        /// Holt oder setzt Größe der Gruppe in Diagramm.
        /// </summary>
        public decimal Quantity
        {
            get;
            set;
        }
    }
}
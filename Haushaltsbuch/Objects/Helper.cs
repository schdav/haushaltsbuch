using System;
using System.IO;

namespace Haushaltsbuch.Objects
{
    /// <summary>
    /// Hilfsklasse, die globale Daten bereitstellt.
    /// </summary>
    public static class Helper
    {
        #region Felder

        /// <summary>
        /// Art eines Eintrags.
        /// </summary>
        public enum TransactionType
        {
            /// <summary>
            /// Eintrag ist Ausgabe.
            /// </summary>
            Outgoing,

            /// <summary>
            /// Eintrag ist Einnahme.
            /// </summary>
            Incoming,

            /// <summary>
            /// Eintrag ist Saldo.
            /// </summary>
            Balance
        }

        #endregion

        #region Eigenschaften

        /// <summary>
        /// Holt Eigenschaft, die Dateinamen der XML-Datei enthält.
        /// </summary>
        public static string XmlFileName
        {
            get;
        }
            = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Haushaltsbuch\\data.xml");

        #endregion
    }
}
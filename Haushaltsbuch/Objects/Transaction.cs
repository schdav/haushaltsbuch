using System;
using System.Globalization;

namespace Haushaltsbuch.Objects
{
    /// <summary>
    /// Klasse, die einen Eintrag repräsentiert.
    /// </summary>
    public class Transaction
    {
        #region Methoden

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="Transaction" /> Klasse.
        /// </summary>
        /// <param name="dateTime">Datum des Eintrags.</param>
        public Transaction(DateTime dateTime)
        {
            Date = dateTime;
        }

        #endregion

        #region Eigenschaften

        /// <summary>
        /// Holt oder setzt Art des Eintrags.
        /// </summary>
        public Helper.TransactionType TransactionType
        {
            get;
            set;
        }

        /// <summary>
        /// Holt oder setzt Beschreibung des Eintrags.
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Holt oder setzt Betrag des Eintrags.
        /// </summary>
        public string Amount
        {
            get;
            set;
        }

        /// <summary>
        /// Formatierter Betrag des Eintrags.
        /// </summary>
        public string AmountString
            => Convert.ToDecimal(Amount, new CultureInfo("de-DE")).ToString("F", CultureInfo.CurrentCulture);

        /// <summary>
        /// Holt oder setzt Kategorie des Eintrags.
        /// </summary>
        public string Category
        {
            get;
            set;
        }

        /// <summary>
        /// Holt Datum des Eintrags.
        /// </summary>
        public DateTime Date
        {
            get;
        }

        /// <summary>
        /// Formatiertes Datum des Eintrags.
        /// </summary>
        public string DateString => Date.ToShortDateString();

        /// <summary>
        /// Tag des Datums.
        /// </summary>
        public string Day => Date.Day.ToString("D2", CultureInfo.InvariantCulture);

        /// <summary>
        /// Monat des Datums.
        /// </summary>
        public string Month => Date.Month.ToString("D2", CultureInfo.InvariantCulture);

        /// <summary>
        /// Jahr des Datums.
        /// </summary>
        public string Year => Date.Year.ToString("D4", CultureInfo.InvariantCulture);

        #endregion
    }
}
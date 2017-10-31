using System;
using System.Globalization;
using System.Linq;
using Haushaltsbuch.Interfaces;
using Haushaltsbuch.Properties;

namespace Haushaltsbuch.Objects
{
    /// <summary>
    /// Klasse, die Schnittstelle zum Berechnen von Daten implementiert.
    /// </summary>
    public class DataCalculator : IDataCalculator
    {
        /// <summary>
        /// Berechnet Saldo von Einträgen.
        /// </summary>
        /// <param name="selectedTransactions">Ausgewählte Einträge.</param>
        /// <returns>Berechnetes Saldo.</returns>
        public decimal CalculateBalance(Transaction[] selectedTransactions)
        {
            return selectedTransactions.Sum(t => Convert.ToDecimal(t.Amount, new CultureInfo("de-DE")));
        }

        /// <summary>
        /// Konvertiert Betrag in Zahl mit zwei Dezimalstellen.
        /// </summary>
        /// <param name="amount">Betrag, der konvertiert werden soll.</param>
        /// <param name="isOutgoing">
        /// <c>true</c> Betrag ist Ausgabe.
        /// <c>false</c> Betrag ist Einnahme.
        /// </param>
        /// <returns>Konvertierter Betrag.</returns>
        public string ConvertAmount(string amount, bool isOutgoing)
        {
            decimal amountDecimal = Math.Abs(Convert.ToDecimal(amount, CultureInfo.CurrentCulture));

            if (isOutgoing)
            {
                amountDecimal = amountDecimal * -1;
            }

            return amountDecimal.ToString("F2", new CultureInfo("de-DE"));
        }

        /// <summary>
        /// Erstellt Filter aus Auswahlen.
        /// </summary>
        /// <param name="month">Ausgewählter Monat.</param>
        /// <param name="year">Ausgewähltes Jahr.</param>
        /// <param name="category">Ausgewählte Kategorie.</param>
        /// <param name="searchTerm">Suchbegriff, nach dem gefiltert werden soll.</param>
        /// <returns>Erstellter Filter.</returns>
        public Filter CreateFilter(int month, string year, string category, string searchTerm)
        {
            Filter filter = new Filter
            {
                Month = string.Format(CultureInfo.InvariantCulture, "{0:00}", month),
                Year = year,
                Category = category,
                SearchTerm = searchTerm
            };

            return filter;
        }

        /// <summary>
        /// Gibt Monate zum Filtern von Einträgen zurück.
        /// </summary>
        /// <returns>Monate zum Filtern von Einträgen.</returns>
        public string[] GetMonths()
        {
            return new[]
            {
                string.Empty, Resources.January, Resources.February, Resources.March, Resources.April, Resources.May,
                Resources.June, Resources.July, Resources.August, Resources.September, Resources.October,
                Resources.November, Resources.December
            };
        }

        /// <summary>
        /// Gibt Jahre zum Filtern von Einträgen zurück.
        /// </summary>
        /// <returns>Jahre zum Filtern von Einträgen.</returns>
        public string[] GetYears()
        {
            return new[]
            {
                (DateTime.Today.Year - 2).ToString(CultureInfo.CurrentCulture),
                (DateTime.Today.Year - 1).ToString(CultureInfo.CurrentCulture),
                DateTime.Today.Year.ToString(CultureInfo.CurrentCulture)
            };
        }

        /// <summary>
        /// Validiert Betrag.
        /// </summary>
        /// <param name="amount">Betrag, der validiert werden soll.</param>
        /// <returns>
        /// <c>true</c> Betrag ist valide.
        /// <c>false</c> Betrag ist nicht valide.
        /// </returns>
        public bool ValidateAmount(string amount)
        {
            decimal result;
            return decimal.TryParse(amount, out result);
        }
    }
}
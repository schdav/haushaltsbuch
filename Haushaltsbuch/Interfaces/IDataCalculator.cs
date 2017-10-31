using Haushaltsbuch.Objects;

namespace Haushaltsbuch.Interfaces
{
    /// <summary>
    /// Schnittstelle, die Funktionen zum Berechnen von Daten anbietet.
    /// </summary>
    public interface IDataCalculator
    {
        /// <summary>
        /// Validiert Betrag.
        /// </summary>
        /// <param name="amount">Betrag, der validiert werden soll.</param>
        /// <returns>
        /// <c>true</c> Betrag ist valide.
        /// <c>false</c> Betrag ist nicht valide.
        /// </returns>
        bool ValidateAmount(string amount);

        /// <summary>
        /// Berechnet Saldo von Einträgen.
        /// </summary>
        /// <param name="selectedTransactions">Ausgewählte Einträge.</param>
        /// <returns>Berechnetes Saldo.</returns>
        decimal CalculateBalance(Transaction[] selectedTransactions);

        /// <summary>
        /// Konvertiert Betrag in Zahl mit zwei Dezimalstellen.
        /// </summary>
        /// <param name="amount">Betrag, der konvertiert werden soll.</param>
        /// <param name="isOutgoing">
        /// <c>true</c> Betrag ist Ausgabe.
        /// <c>false</c> Betrag ist Einnahme.
        /// </param>
        /// <returns>Konvertierter Betrag.</returns>
        string ConvertAmount(string amount, bool isOutgoing);

        /// <summary>
        /// Gibt Monate zum Filtern von Einträgen zurück.
        /// </summary>
        /// <returns>Monate zum Filtern von Einträgen.</returns>
        string[] GetMonths();

        /// <summary>
        /// Gibt Jahre zum Filtern von Einträgen zurück.
        /// </summary>
        /// <returns>Jahre zum Filtern von Einträgen.</returns>
        string[] GetYears();

        /// <summary>
        /// Erstellt Filter aus Auswahlen.
        /// </summary>
        /// <param name="month">Ausgewählter Monat.</param>
        /// <param name="year">Ausgewähltes Jahr.</param>
        /// <param name="category">Ausgewählte Kategorie.</param>
        /// <param name="searchTerm">Suchbegriff, nach dem gefiltert werden soll.</param>
        /// <returns>Erstellter Filter.</returns>
        Filter CreateFilter(int month, string year, string category, string searchTerm);
    }
}
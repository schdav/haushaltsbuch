using Haushaltsbuch.Objects;

namespace Haushaltsbuch.Interfaces
{
    /// <summary>
    /// Schnittstelle, die Funktionen zum Lesen aus XML-Datei anbietet.
    /// </summary>
    public interface IXmlFileReader
    {
        /// <summary>
        /// Liest Beschreibungen von Ausgaben aus XML-Datei.
        /// </summary>
        /// <param name="fileName">Dateiname der XML-Datei.</param>
        /// <returns>Beschreibungen der Ausgaben.</returns>
        string[] GetDescriptionsFromOutgoingTransactions(string fileName);

        /// <summary>
        /// Liest Beschreibungen von Einnahmen aus XML-Datei.
        /// </summary>
        /// <param name="fileName">Dateiname der XML-Datei.</param>
        /// <returns>Beschreibungen der Einnahmen.</returns>
        string[] GetDescriptionsFromIncomingTransactions(string fileName);

        /// <summary>
        /// Liest Kategorien von Ausgaben aus XML-Datei.
        /// </summary>
        /// <param name="fileName">Dateiname der XML-Datei.</param>
        /// <returns>Kategorien der Ausgaben.</returns>
        string[] GetCategoriesFromOutgoingTransactions(string fileName);

        /// <summary>
        /// Liest Kategorien von Einnahmen aus XML-Datei.
        /// </summary>
        /// <param name="fileName">Dateiname der XML-Datei.</param>
        /// <returns>Kategorien der Einnahmen.</returns>
        string[] GetCategoriesFromIncomingTransactions(string fileName);

        /// <summary>
        /// Liest Kategorien aller Einträge aus XML-Datei.
        /// </summary>
        /// <param name="fileName">Dateiname der XML-Datei.</param>
        /// <returns>Kategorien aller Einträge.</returns>
        string[] GetCategoriesFromAllTransactions(string fileName);

        /// <summary>
        /// Liest Einträge aus XML-Datei.
        /// </summary>
        /// <param name="fileName">Dateiname der XML-Datei.</param>
        /// <param name="filter">Filter, der auf Einträge angewendet wird.</param>
        /// <returns>Einträge aus XML-Datei.</returns>
        Transaction[] GetTransactions(string fileName, Filter filter);
    }
}
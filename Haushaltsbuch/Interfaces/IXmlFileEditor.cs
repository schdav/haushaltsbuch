using Haushaltsbuch.Objects;

namespace Haushaltsbuch.Interfaces
{
    /// <summary>
    /// Schnittstelle, die Funktionen zum Bearbeiten von XML-Datei anbietet.
    /// </summary>
    public interface IXmlFileEditor
    {
        /// <summary>
        /// Schreibt Eintrag in XML-Datei.
        /// </summary>
        /// <param name="fileName">Dateiname der XML-Datei.</param>
        /// <param name="transaction">Eintrag, der in XML-Datei geschrieben werden soll.</param>
        void WriteTransaction(string fileName, Transaction transaction);

        /// <summary>
        /// Entfernt Eintrag aus XML-Datei.
        /// </summary>
        /// <param name="fileName">Dateiname der XML-Datei.</param>
        /// <param name="transaction">Eintrag, der aus XML-Datei entfernt werden soll.</param>
        void DeleteTransaction(string fileName, Transaction transaction);

        /// <summary>
        /// Prüft, ob XML-Datei vorhanden ist.
        /// </summary>
        /// <param name="fileName">Dateiname der XML-Datei.</param>
        /// <returns>
        /// <c>true</c> XML-Datei ist vorhanden.
        /// <c>false</c> XML-Datei ist nicht vorhanden.
        /// </returns>
        bool CheckXmlDocument(string fileName);

        /// <summary>
        /// Erstellt XML-Datei.
        /// </summary>
        /// <param name="fileName">Dateiname der XML-Datei.</param>
        void CreateXmlDocument(string fileName);
    }
}
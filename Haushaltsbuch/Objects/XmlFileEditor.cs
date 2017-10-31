using System.IO;
using System.Linq;
using System.Xml;
using Haushaltsbuch.Interfaces;

namespace Haushaltsbuch.Objects
{
    /// <summary>
    /// Klasse, die Schnittstelle zum Bearbeiten von XML-Datei implementiert.
    /// </summary>
    public class XmlFileEditor : IXmlFileEditor
    {
        /// <summary>
        /// Prüft, ob XML-Datei vorhanden ist.
        /// </summary>
        /// <param name="fileName">Dateiname der XML-Datei.</param>
        /// <returns>
        /// <c>true</c> XML-Datei ist vorhanden.
        /// <c>false</c> XML-Datei ist nicht vorhanden.
        /// </returns>
        public bool CheckXmlDocument(string fileName)
        {
            return File.Exists(fileName);
        }

        /// <summary>
        /// Erstellt XML-Datei.
        /// </summary>
        /// <param name="fileName">Dateiname der XML-Datei.</param>
        public void CreateXmlDocument(string fileName)
        {
            string directory = Path.GetDirectoryName(fileName);

            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            XmlDocument xmlDocument = new XmlDocument();

            XmlNode declarationNode = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDocument.AppendChild(declarationNode);

            XmlNode transactionsNode = xmlDocument.CreateElement("transactions");
            xmlDocument.AppendChild(transactionsNode);

            xmlDocument.Save(fileName);
        }

        /// <summary>
        /// Entfernt Eintrag aus XML-Datei.
        /// </summary>
        /// <param name="fileName">Dateiname der XML-Datei.</param>
        /// <param name="transaction">Eintrag, der aus XML-Datei entfernt werden soll.</param>
        public void DeleteTransaction(string fileName, Transaction transaction)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);

            XmlNode[] selectedTransactionNodes =
            (from XmlNode transactionNode in xmlDocument.SelectNodes("transactions/transaction")
                let transactionType =
                string.Equals(transactionNode.SelectSingleNode("type")?.InnerText, "outgoing")
                    ? Helper.TransactionType.Outgoing
                    : Helper.TransactionType.Incoming
                where
                transactionType == transaction.TransactionType &&
                string.Equals(transactionNode.SelectSingleNode("day")?.InnerText, transaction.Day) &&
                string.Equals(transactionNode.SelectSingleNode("month")?.InnerText, transaction.Month) &&
                string.Equals(transactionNode.SelectSingleNode("year")?.InnerText, transaction.Year) &&
                string.Equals(
                    transactionNode.SelectSingleNode("description")?.InnerText,
                    transaction.Description) &&
                string.Equals(transactionNode.SelectSingleNode("amount")?.InnerText, transaction.Amount) &&
                string.Equals(transactionNode.SelectSingleNode("category")?.InnerText, transaction.Category)
                select transactionNode).ToArray();

            if (selectedTransactionNodes.Length == 0)
            {
                return;
            }

            // Entfernt immer ersten gefundenen Eintrag.
            XmlNode parentNode = selectedTransactionNodes[0].ParentNode;

            parentNode?.RemoveChild(selectedTransactionNodes[0]);

            xmlDocument.Save(fileName);
        }

        /// <summary>
        /// Schreibt Eintrag in XML-Datei.
        /// </summary>
        /// <param name="fileName">Dateiname der XML-Datei.</param>
        /// <param name="transaction">Eintrag, der in XML-Datei geschrieben werden soll.</param>
        public void WriteTransaction(string fileName, Transaction transaction)
        {
            if (transaction == null)
            {
                return;
            }

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);

            XmlNode transactionsNode = xmlDocument.SelectSingleNode("transactions");

            XmlNode transactionNode = xmlDocument.CreateElement("transaction");

            transactionsNode?.AppendChild(transactionNode);

            XmlNode typeNode = xmlDocument.CreateElement("type");
            typeNode.InnerText = transaction.TransactionType == Helper.TransactionType.Outgoing
                ? "outgoing"
                : "incoming";
            transactionNode.AppendChild(typeNode);

            XmlNode dayNode = xmlDocument.CreateElement("day");
            dayNode.InnerText = transaction.Day;
            transactionNode.AppendChild(dayNode);

            XmlNode monthNode = xmlDocument.CreateElement("month");
            monthNode.InnerText = transaction.Month;
            transactionNode.AppendChild(monthNode);

            XmlNode yearNode = xmlDocument.CreateElement("year");
            yearNode.InnerText = transaction.Year;
            transactionNode.AppendChild(yearNode);

            XmlNode descriptionNode = xmlDocument.CreateElement("description");
            descriptionNode.InnerText = transaction.Description;
            transactionNode.AppendChild(descriptionNode);

            XmlNode amountNode = xmlDocument.CreateElement("amount");
            amountNode.InnerText = transaction.Amount;
            transactionNode.AppendChild(amountNode);

            XmlNode categoryNode = xmlDocument.CreateElement("category");
            categoryNode.InnerText = transaction.Category;
            transactionNode.AppendChild(categoryNode);

            xmlDocument.Save(fileName);
        }
    }
}
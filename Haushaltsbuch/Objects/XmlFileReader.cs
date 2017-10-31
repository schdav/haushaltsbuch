using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using Haushaltsbuch.Interfaces;

namespace Haushaltsbuch.Objects
{
    /// <summary>
    /// Klasse, die Schnittstelle zum Lesen aus XML-Datei implementiert.
    /// </summary>
    public class XmlFileReader : IXmlFileReader
    {
        /// <summary>
        /// Liest Kategorien aller Einträge aus XML-Datei.
        /// </summary>
        /// <param name="fileName">Dateiname der XML-Datei.</param>
        /// <returns>Kategorien aller Einträge.</returns>
        public string[] GetCategoriesFromAllTransactions(string fileName)
        {
            string[] emptyList =
            {
                string.Empty
            };

            string[] categoryList =
            (from string category in
                ReadCategoriesFromOutgoingTransactions(fileName)
                    .Concat(ReadCategoriesFromIncomingTransactions(fileName).Concat(emptyList))
                orderby category
                select category).Distinct().ToArray();

            return categoryList;
        }

        /// <summary>
        /// Liest Kategorien von Einnahmen aus XML-Datei.
        /// </summary>
        /// <param name="fileName">Dateiname der XML-Datei.</param>
        /// <returns>Kategorien der Einnahmen.</returns>
        public string[] GetCategoriesFromIncomingTransactions(string fileName)
        {
            return ReadCategoriesFromIncomingTransactions(fileName);
        }

        /// <summary>
        /// Liest Kategorien von Ausgaben aus XML-Datei.
        /// </summary>
        /// <param name="fileName">Dateiname der XML-Datei.</param>
        /// <returns>Kategorien der Ausgaben.</returns>
        public string[] GetCategoriesFromOutgoingTransactions(string fileName)
        {
            return ReadCategoriesFromOutgoingTransactions(fileName);
        }

        /// <summary>
        /// Liest Beschreibungen von Einnahmen aus XML-Datei.
        /// </summary>
        /// <param name="fileName">Dateiname der XML-Datei.</param>
        /// <returns>Beschreibungen der Einnahmen.</returns>
        public string[] GetDescriptionsFromIncomingTransactions(string fileName)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);

            const string XPATH = "transactions/transaction[type='incoming']/description";
            XmlNodeList descriptionNodesList = xmlDocument.SelectNodes(XPATH);

            string[] descriptionsList = (from XmlNode descriptionNode in descriptionNodesList
                where !string.IsNullOrEmpty(descriptionNode.InnerText)
                orderby descriptionNode.InnerText
                select descriptionNode.InnerText).Distinct().ToArray();

            return descriptionsList;
        }

        /// <summary>
        /// Liest Beschreibungen von Ausgaben aus XML-Datei.
        /// </summary>
        /// <param name="fileName">Dateiname der XML-Datei.</param>
        /// <returns>Beschreibungen der Ausgaben.</returns>
        public string[] GetDescriptionsFromOutgoingTransactions(string fileName)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);

            const string XPATH = "transactions/transaction[type='outgoing']/description";
            XmlNodeList descriptionNodesList = xmlDocument.SelectNodes(XPATH);

            string[] descriptionsList = (from XmlNode descriptionNode in descriptionNodesList
                where !string.IsNullOrEmpty(descriptionNode.InnerText)
                orderby descriptionNode.InnerText
                select descriptionNode.InnerText).Distinct().ToArray();

            return descriptionsList;
        }

        /// <summary>
        /// Liest Einträge aus XML-Datei.
        /// </summary>
        /// <param name="fileName">Dateiname der XML-Datei.</param>
        /// <param name="filter">Filter, der auf Einträge angewendet wird.</param>
        /// <returns>Einträge aus XML-Datei.</returns>
        public Transaction[] GetTransactions(string fileName, Filter filter)
        {
            IEnumerable<XmlNode> filteredTransactions = FilterTransactions(fileName, filter);

            Transaction[] selectedTransactions = (from XmlNode transactionNode in filteredTransactions
                let typeNode = transactionNode.SelectSingleNode("type")
                let dayNode = transactionNode.SelectSingleNode("day")
                let monthNode = transactionNode.SelectSingleNode("month")
                let yearNode = transactionNode.SelectSingleNode("year")
                let descriptionNode = transactionNode.SelectSingleNode("description")
                let amountNode = transactionNode.SelectSingleNode("amount")
                let categoryNode = transactionNode.SelectSingleNode("category")
                select
                new Transaction(
                    new DateTime(
                        Convert.ToInt32(yearNode.InnerText, CultureInfo.InvariantCulture),
                        Convert.ToInt32(monthNode.InnerText, CultureInfo.InvariantCulture),
                        Convert.ToInt32(dayNode.InnerText, CultureInfo.InvariantCulture)))
                {
                    Amount = amountNode.InnerText,
                    Category = categoryNode.InnerText,
                    Description = descriptionNode.InnerText,
                    TransactionType =
                        string.Equals(typeNode.InnerText, "outgoing")
                            ? Helper.TransactionType.Outgoing
                            : Helper.TransactionType.Incoming
                }).OrderBy(t => t.Date).ToArray();

            return selectedTransactions;
        }

        /// <summary>
        /// Liest Kategorien von Einnahmen aus XML-Datei.
        /// </summary>
        /// <param name="fileName">Dateiname der XML-Datei.</param>
        /// <returns>Kategorien der Einnahmen.</returns>
        private static string[] ReadCategoriesFromIncomingTransactions(string fileName)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);

            const string XPATH = "transactions/transaction[type='incoming']/category";
            XmlNodeList categoryNodesList = xmlDocument.SelectNodes(XPATH);

            string[] categoriesList = (from XmlNode categoryNode in categoryNodesList
                where !string.IsNullOrEmpty(categoryNode.InnerText)
                orderby categoryNode.InnerText
                select categoryNode.InnerText).Distinct().ToArray();

            return categoriesList;
        }

        /// <summary>
        /// Liest Kategorien von Ausgaben aus XML-Datei.
        /// </summary>
        /// <param name="fileName">Dateiname der XML-Datei.</param>
        /// <returns>Kategorien der Ausgaben.</returns>
        private static string[] ReadCategoriesFromOutgoingTransactions(string fileName)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);

            const string XPATH = "transactions/transaction[type='outgoing']/category";
            XmlNodeList categoryNodesList = xmlDocument.SelectNodes(XPATH);

            string[] categoriesList = (from XmlNode categoryNode in categoryNodesList
                where !string.IsNullOrEmpty(categoryNode.InnerText)
                orderby categoryNode.InnerText
                select categoryNode.InnerText).Distinct().ToArray();

            return categoriesList;
        }

        /// <summary>
        /// Filtert Einträge in XML-Datei.
        /// </summary>
        /// <param name="fileName">Dateiname der XML-Datei.</param>
        /// <param name="filter">Filter, der auf Einträge angewendet wird.</param>
        /// <returns>Gefilterte Einträge.</returns>
        private static IEnumerable<XmlNode> FilterTransactions(string fileName, Filter filter)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);

            XmlNode[] filteredTransactions =
            (from XmlNode transactionNode in xmlDocument.SelectNodes("transactions/transaction")
                let yearNode = transactionNode.SelectSingleNode("year")
                where string.Equals(yearNode.InnerText, filter.Year)
                select transactionNode).ToArray();

            if (!string.Equals(filter.Month, "00"))
            {
                filteredTransactions = ApplyMonth(filter, filteredTransactions);
            }

            if (!string.IsNullOrEmpty(filter.Category))
            {
                filteredTransactions = ApplyCategory(filter, filteredTransactions);
            }

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                filteredTransactions = ApplySearchTerm(filter, filteredTransactions);
            }

            return filteredTransactions;
        }

        /// <summary>
        /// Wendet Suchbegriff als Filter an.
        /// </summary>
        /// <param name="filter">Filter, der auf Einträge angewendet wird.</param>
        /// <param name="filteredTransactions">Bereits gefilterte Einträge.</param>
        /// <returns>Gefilterte Einträge.</returns>
        private static XmlNode[] ApplySearchTerm(Filter filter, XmlNode[] filteredTransactions)
        {
            filteredTransactions = (from XmlNode transactionNode in filteredTransactions
                let descriptionNode = transactionNode.SelectSingleNode("description")
                where
                descriptionNode.InnerText.ToLower(CultureInfo.CurrentCulture)
                    .Contains(filter.SearchTerm.ToLower(CultureInfo.CurrentCulture))
                select transactionNode).ToArray();
            return filteredTransactions;
        }

        /// <summary>
        /// Wendet Kategorie als Filter an.
        /// </summary>
        /// <param name="filter">Filter, der auf Einträge angewendet wird.</param>
        /// <param name="filteredTransactions">Bereits gefilterte Einträge.</param>
        /// <returns>Gefilterte Einträge.</returns>
        private static XmlNode[] ApplyCategory(Filter filter, XmlNode[] filteredTransactions)
        {
            filteredTransactions = (from XmlNode transactionNode in filteredTransactions
                let categoryNode = transactionNode.SelectSingleNode("category")
                where string.Equals(categoryNode.InnerText, filter.Category)
                select transactionNode).ToArray();
            return filteredTransactions;
        }

        /// <summary>
        /// Wendet Monat als Filter an.
        /// </summary>
        /// <param name="filter">Filter, der auf Einträge angewendet wird.</param>
        /// <param name="filteredTransactions">Bereits gefilterte Einträge.</param>
        /// <returns>Gefiltertete Einträge.</returns>
        private static XmlNode[] ApplyMonth(Filter filter, XmlNode[] filteredTransactions)
        {
            filteredTransactions = (from XmlNode transactionNode in filteredTransactions
                let monthNode = transactionNode.SelectSingleNode("month")
                where string.Equals(monthNode.InnerText, filter.Month)
                select transactionNode).ToArray();
            return filteredTransactions;
        }
    }
}
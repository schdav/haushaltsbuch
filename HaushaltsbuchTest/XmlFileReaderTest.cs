using System.Collections.Specialized;
using Haushaltsbuch.Interfaces;
using Haushaltsbuch.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HaushaltsbuchTest
{
    /// <summary>
    /// Testklasse, die Klasse zum Lesen aus XML-Datei testet.
    /// </summary>
    [TestClass]
    [DeploymentItem("TestDocuments\\SampleXmlDocument.xml", "XmlFileReaderTest")]
    [DeploymentItem("TestDocuments\\EmptyXmlDocument.xml", "XmlFileReaderTest")]
    public class XmlFileReaderTest
    {
        #region Felder

        /// <summary>
        /// Speicherort von Testdatei mit Beispieldaten.
        /// </summary>
        private const string SampleXmlDocument = "XmlFileReaderTest\\SampleXmlDocument.xml";

        /// <summary>
        /// Speicherort von leerer Testdatei.
        /// </summary>
        private const string EmptyXmlDocument = "XmlFileReaderTest\\EmptyXmlDocument.xml";

        /// <summary>
        /// Klasse zum Lesen aus XML-Datei.
        /// </summary>
        private IXmlFileReader xmlFileReader;

        #endregion

        #region Methoden

        /// <summary>
        /// Initialisiert Klasse zum Lesen aus XML-Datei, sowie Testdateien.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            xmlFileReader = new XmlFileReader();
        }

        /// <summary>
        /// Testet Lesen von Beschreibungen von Ausgaben aus XML-Datei.
        /// </summary>
        [TestMethod]
        public void TestGetDescriptionsFromOutgoingTransactions()
        {
            // Arrange
            string[] expectedDescriptions =
            {
                "outgoingDescription1", "outgoingDescription2"
            };

            // Act
            string[] actualDescriptions = xmlFileReader.GetDescriptionsFromOutgoingTransactions(SampleXmlDocument);

            // Assert
            CollectionAssert.AreEqual(expectedDescriptions, actualDescriptions);
        }

        /// <summary>
        /// Testet Lesen von Beschreibungen von Ausgaben aus XML-Datei mit leerer Datei.
        /// </summary>
        [TestMethod]
        public void TestGetDescriptionsFromOutgoingTransactionsWithEmptyFile()
        {
            // Arrange
            string[] expectedDescriptions = new string[0];

            // Act
            string[] actualDescriptions = xmlFileReader.GetDescriptionsFromOutgoingTransactions(EmptyXmlDocument);

            // Assert
            CollectionAssert.AreEqual(expectedDescriptions, actualDescriptions);
        }

        /// <summary>
        /// Testet Lesen von Beschreibungen von Einnahmen aus XML-Datei.
        /// </summary>
        [TestMethod]
        public void TestGetDescriptionsFromIncomingTransactions()
        {
            // Arrange
            string[] expectedDescriptions =
            {
                "incomingDescription1", "incomingDescription2"
            };

            // Act
            string[] actualDescriptions = xmlFileReader.GetDescriptionsFromIncomingTransactions(SampleXmlDocument);

            // Assert
            CollectionAssert.AreEqual(expectedDescriptions, actualDescriptions);
        }

        /// <summary>
        /// Testet Lesen von Beschreibungen von Einnahmen aus XML-Datei mit leerer Datei.
        /// </summary>
        [TestMethod]
        public void TestGetDescriptionsFromIncomingTransactionsWithEmptyFile()
        {
            // Arrange
            StringCollection[] expectedDescriptions = new StringCollection[0];

            // Act
            string[] actualDescriptions = xmlFileReader.GetDescriptionsFromIncomingTransactions(EmptyXmlDocument);

            // Assert
            CollectionAssert.AreEqual(expectedDescriptions, actualDescriptions);
        }

        /// <summary>
        /// Testet Lesen von Kategorien von Ausgaben aus XML-Datei.
        /// </summary>
        [TestMethod]
        public void TestGetCategoriesFromOutgoingTransactions()
        {
            // Arrange
            string[] expectedCategories =
            {
                "outgoingCategory1", "outgoingCategory2"
            };

            // Act
            string[] actualCategories = xmlFileReader.GetCategoriesFromOutgoingTransactions(SampleXmlDocument);

            // Assert
            CollectionAssert.AreEqual(expectedCategories, actualCategories);
        }

        /// <summary>
        /// Testet Lesen von Kategorien von Ausgaben aus XML-Datei mit leerer Datei.
        /// </summary>
        [TestMethod]
        public void TestGetCategoriesFromOutgoingTransactionsWithEmptyFile()
        {
            // Arrange
            string[] expectedCategories = new string[0];

            // Act
            string[] actualCategories = xmlFileReader.GetDescriptionsFromOutgoingTransactions(EmptyXmlDocument);

            // Assert
            CollectionAssert.AreEqual(expectedCategories, actualCategories);
        }

        /// <summary>
        /// Testet Lesen von Kategorien von Einnahmen aus XML-Datei.
        /// </summary>
        [TestMethod]
        public void TestGetCategoriesFromIncomingTransactions()
        {
            // Arrange
            string[] expectedCategories =
            {
                "incomingCategory1", "incomingCategory2"
            };

            // Act
            string[] actualCategories = xmlFileReader.GetCategoriesFromIncomingTransactions(SampleXmlDocument);

            // Assert
            CollectionAssert.AreEqual(expectedCategories, actualCategories);
        }

        /// <summary>
        /// Testet Lesen von Kategorien von Einnahmen aus XML-Datei mit leerer Datei.
        /// </summary>
        [TestMethod]
        public void TestGetCategoriesFromIncomingTransactionsWithEmptyFile()
        {
            // Arrange
            string[] expectedCategories = new string[0];

            // Act
            string[] actualCategories = xmlFileReader.GetCategoriesFromIncomingTransactions(EmptyXmlDocument);

            // Assert
            CollectionAssert.AreEqual(expectedCategories, actualCategories);
        }

        /// <summary>
        /// Testet Lesen von Kategorien aller Einträge aus XML-Datei.
        /// </summary>
        [TestMethod]
        public void GetCategoriesFromAllTransactions()
        {
            // Arrange
            string[] expectedCategories =
            {
                string.Empty, "incomingCategory1", "incomingCategory2", "outgoingCategory1", "outgoingCategory2"
            };

            // Act
            string[] actualCategories = xmlFileReader.GetCategoriesFromAllTransactions(SampleXmlDocument);

            // Assert
            CollectionAssert.AreEqual(expectedCategories, actualCategories);
        }

        /// <summary>
        /// Testet Lesen von Kategorien aller Einträge aus XML-Datei mit leerer Datei.
        /// </summary>
        [TestMethod]
        public void TestGetCategoriesFromAllTransactionsWithEmptyFile()
        {
            // Arrange
            string[] expectedCategories =
            {
                string.Empty
            };

            // Act
            string[] actualCategories = xmlFileReader.GetCategoriesFromAllTransactions(EmptyXmlDocument);

            // Assert
            CollectionAssert.AreEqual(expectedCategories, actualCategories);
        }

        /// <summary>
        /// Testet Lesen von Einträgen aus XML-Datei ohne Filter.
        /// </summary>
        [TestMethod]
        public void TestGetTransactionsWithoutCategoryFilter()
        {
            // Arrange
            Filter filter = new Filter
            {
                Category = string.Empty,
                Month = "11",
                Year = "2013",
                SearchTerm = string.Empty
            };

            // Act
            int numberOfTransactions = xmlFileReader.GetTransactions(SampleXmlDocument, filter).Length;

            // Assert
            Assert.AreEqual(8, numberOfTransactions);
        }

        /// <summary>
        /// Testet Lesen von Einträgen aus XML-Datei mit Filter.
        /// </summary>
        [TestMethod]
        public void TestGetTransactionsWithCategoryFilter()
        {
            // Arrange
            Filter filter = new Filter
            {
                Category = "outgoingCategory2",
                Month = "11",
                Year = "2013",
                SearchTerm = string.Empty
            };

            // Act
            int numberOfTransactions = xmlFileReader.GetTransactions(SampleXmlDocument, filter).Length;

            // Assert
            Assert.AreEqual(2, numberOfTransactions);
        }

        /// <summary>
        /// Testet Lesen von allen Einträgen eines Jahres aus XML-Datei.
        /// </summary>
        [TestMethod]
        public void TestGetAllTransactionsOfYear()
        {
            // Arrange
            Filter filter = new Filter
            {
                Category = string.Empty,
                Month = "00",
                Year = "2013",
                SearchTerm = string.Empty
            };

            // Act
            int numberOfTransactions = xmlFileReader.GetTransactions(SampleXmlDocument, filter).Length;

            // Assert
            Assert.AreEqual(9, numberOfTransactions);
        }

        /// <summary>
        /// Testet Lesen von Einträgen aus XML-Datei mit Suchbegriff als Filter.
        /// </summary>
        [TestMethod]
        public void TestGetTransactionsWithSearchFilter()
        {
            // Arrange
            Filter filter = new Filter
            {
                Category = string.Empty,
                Month = "00",
                Year = "2013",
                SearchTerm = "outgoingDescription2"
            };

            // Act
            int numberOfTransactions = xmlFileReader.GetTransactions(SampleXmlDocument, filter).Length;

            // Assert
            Assert.AreEqual(3, numberOfTransactions);
        }

        /// <summary>
        /// Testet Lesen von Einträgen aus XML-Datei mit leerer Datei.
        /// </summary>
        [TestMethod]
        public void TestGetTransactionsWithEmptyFile()
        {
            // Arrange
            Filter filter = new Filter
            {
                Category = "outgoingCategory2",
                Month = "11",
                Year = "2013",
                SearchTerm = string.Empty
            };

            // Act
            int numberOfTransactions = xmlFileReader.GetTransactions(EmptyXmlDocument, filter).Length;

            // Assert
            Assert.AreEqual(0, numberOfTransactions);
        }

        #endregion
    }
}
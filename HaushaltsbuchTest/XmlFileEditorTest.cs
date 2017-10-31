using System;
using System.IO;
using System.Xml;
using Haushaltsbuch.Interfaces;
using Haushaltsbuch.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HaushaltsbuchTest
{
    /// <summary>
    /// Testklasse, die Klasse zum Bearbeiten von XML-Datei testet.
    /// </summary>
    [TestClass]
    [DeploymentItem("TestDocuments\\ComparisonXmlDocument.xml", "XmlFileEditorTest")]
    [DeploymentItem("TestDocuments\\EmptyXmlDocument.xml", "XmlFileEditorTest")]
    [DeploymentItem("TestDocuments\\SampleXmlDocumentForDeletion.xml", "XmlFileEditorTest")]
    public class XmlFileEditorTest
    {
        #region Felder

        /// <summary>
        /// Speicherort von Testdatei zum Vergleichen.
        /// </summary>
        private const string ComparisonXmlDocument = "XmlFileEditorTest\\ComparisonXmlDocument.xml";

        /// <summary>
        /// Speicherort von leerer Testdatei.
        /// </summary>
        private const string EmptyXmlDocument = "XmlFileEditorTest\\EmptyXmlDocument.xml";

        /// <summary>
        /// Speicherort von Testdatei mit Beispieldaten zum Löschen.
        /// </summary>
        private const string SampleXmlDocumentForDeletion = "XmlFileEditorTest\\SampleXmlDocumentForDeletion.xml";

        /// <summary>
        /// Testdatei zum Vergleichen.
        /// </summary>
        private XmlDocument comparisonXmlDocument;

        /// <summary>
        /// Leere Testdatei.
        /// </summary>
        private XmlDocument emptyXmlDocument;

        /// <summary>
        /// Testdatei mit Beispieldaten zum Löschen.
        /// </summary>
        private XmlDocument sampleXmlDocumentForDeletion;

        /// <summary>
        /// Klasse zum Bearbeiten von XML-Datei.
        /// </summary>
        private IXmlFileEditor xmlFileEditor;

        #endregion

        #region Methoden

        /// <summary>
        /// Initialisiert Klasse zum Bearbeiten von XML-Datei, sowie Testdateien.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            xmlFileEditor = new XmlFileEditor();
            comparisonXmlDocument = new XmlDocument();
            emptyXmlDocument = new XmlDocument();
            sampleXmlDocumentForDeletion = new XmlDocument();
        }

        /// <summary>
        /// Testet Schreiben von Eintrag in XML-Datei.
        /// </summary>
        [TestMethod]
        public void TestWriteTransaction()
        {
            // Arrange
            DateTime dateTime = new DateTime(2013, 11, 23);

            Transaction transaction1 = new Transaction(dateTime)
            {
                TransactionType = Helper.TransactionType.Outgoing,
                Amount = "-100,00",
                Category = "outgoingCategory",
                Description = "outgoingDescription"
            };

            Transaction transaction2 = new Transaction(dateTime)
            {
                TransactionType = Helper.TransactionType.Outgoing,
                Amount = "-100,00",
                Category = string.Empty,
                Description = string.Empty
            };

            string expected = "expected";
            string actual = "actual";

            // Act
            xmlFileEditor.WriteTransaction(EmptyXmlDocument, transaction1);
            xmlFileEditor.WriteTransaction(EmptyXmlDocument, transaction2);

            comparisonXmlDocument.Load(ComparisonXmlDocument);
            emptyXmlDocument.Load(EmptyXmlDocument);

            XmlNode comparisonXmlDocumentTransactionsNode = comparisonXmlDocument.SelectSingleNode("transactions");
            if (comparisonXmlDocumentTransactionsNode != null)
            {
                expected = comparisonXmlDocumentTransactionsNode.InnerText;
            }

            XmlNode emptyXmlDocumentTransactionsNode = emptyXmlDocument.SelectSingleNode("transactions");
            if (emptyXmlDocumentTransactionsNode != null)
            {
                actual = emptyXmlDocumentTransactionsNode.InnerText;
            }

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Testet Schreiben von leerem Eintrag in XML-Datei.
        /// </summary>
        [TestMethod]
        public void TestWriteEmptyTransaction()
        {
            // Arrange
            string expected = "expected";
            string actual = "actual";

            emptyXmlDocument.Load(EmptyXmlDocument);

            XmlNode emptyXmlDocumentTransactionsNode = emptyXmlDocument.SelectSingleNode("transactions");
            if (emptyXmlDocumentTransactionsNode != null)
            {
                expected = emptyXmlDocumentTransactionsNode.InnerText;
            }

            // Act
            xmlFileEditor.WriteTransaction(EmptyXmlDocument, null);

            emptyXmlDocumentTransactionsNode = emptyXmlDocument.SelectSingleNode("transactions");
            if (emptyXmlDocumentTransactionsNode != null)
            {
                actual = emptyXmlDocumentTransactionsNode.InnerText;
            }

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Testet Entfernen von Eintrag aus XML-Datei.
        /// </summary>
        [TestMethod]
        public void TestDeleteTransaction()
        {
            // Arrange
            DateTime dateTime = new DateTime(2013, 11, 23);

            Transaction transaction = new Transaction(dateTime)
            {
                TransactionType = Helper.TransactionType.Outgoing,
                Amount = "-100,00",
                Category = "outgoingCategory",
                Description = "outgoingDescription"
            };

            string expected = string.Empty;
            string actual = "actual";

            // Act
            xmlFileEditor.DeleteTransaction(SampleXmlDocumentForDeletion, transaction);
            xmlFileEditor.DeleteTransaction(SampleXmlDocumentForDeletion, transaction);

            sampleXmlDocumentForDeletion.Load(SampleXmlDocumentForDeletion);

            XmlNode transactionsNode = sampleXmlDocumentForDeletion.SelectSingleNode("transactions");
            if (transactionsNode != null)
            {
                actual = transactionsNode.InnerText;
            }

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Testet Entfernen von nicht vorhandenem Eintrag aus XML-Datei.
        /// </summary>
        [TestMethod]
        public void TestDeleteNotExistingTransaction()
        {
            // Arrange
            string expected = "expected";
            string actual = "actual";

            sampleXmlDocumentForDeletion.Load(SampleXmlDocumentForDeletion);

            XmlNode transactionsNode = sampleXmlDocumentForDeletion.SelectSingleNode("transactions");
            if (transactionsNode != null)
            {
                expected = transactionsNode.InnerText;
            }

            DateTime dateTime = new DateTime(2013, 11, 23);

            Transaction transaction = new Transaction(dateTime)
            {
                TransactionType = Helper.TransactionType.Outgoing,
                Amount = "-100,00",
                Category = "notExisting",
                Description = "notExisting"
            };

            // Act
            xmlFileEditor.DeleteTransaction(SampleXmlDocumentForDeletion, transaction);

            transactionsNode = sampleXmlDocumentForDeletion.SelectSingleNode("transactions");
            if (transactionsNode != null)
            {
                actual = transactionsNode.InnerText;
            }

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Testet Prüfen, ob XML-Datei vorhanden mit nicht vorhandener XML-Datei.
        /// </summary>
        [TestMethod]
        public void TestCheckXmlDocumentWithNotExistingFile()
        {
            // Assert
            const string FILENAME = "null.xml";

            // Act
            bool actual = xmlFileEditor.CheckXmlDocument(FILENAME);

            // Assert
            Assert.IsFalse(actual);
        }

        /// <summary>
        /// Testet Prüfen, ob XML-Datei vorhanden mit vorhandener XML-Datei.
        /// </summary>
        [TestMethod]
        public void TestCheckXmlDocumentWithExistingFile()
        {
            Assert.IsTrue(xmlFileEditor.CheckXmlDocument(EmptyXmlDocument));
        }

        /// <summary>
        /// Testet Erstellen von XML-Datei.
        /// </summary>
        [TestMethod]
        public void TestCreateXmlDocument()
        {
            // Arrange
            const string FILENAME = "Haushaltsbuch\\test.xml";

            // Act
            xmlFileEditor.CreateXmlDocument(FILENAME);

            // Assert
            Assert.IsTrue(File.Exists(FILENAME));
        }

        #endregion
    }
}
using System;
using Haushaltsbuch.Interfaces;
using Haushaltsbuch.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HaushaltsbuchTest
{
    /// <summary>
    /// Testklasse, die Klasse zum Berechnen von Daten testet.
    /// </summary>
    [TestClass]
    public class DataCalculatorTest
    {
        #region Felder

        /// <summary>
        /// Klasse zum Berechnen von Daten.
        /// </summary>
        private IDataCalculator dataCalculator;

        #endregion

        #region methoden

        /// <summary>
        /// Initialisiert Klasse zum Berechnen von Daten.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            dataCalculator = new DataCalculator();
        }

        /// <summary>
        /// Testet Validieren eines Betrags mit leerer Zeichenkette.
        /// </summary>
        [TestMethod]
        public void TestValidateAmountWithEmptyString()
        {
            Assert.IsFalse(dataCalculator.ValidateAmount(string.Empty));
        }

        /// <summary>
        /// Testet Validieren eines Betrags mit Buchstaben.
        /// </summary>
        [TestMethod]
        public void TestValidateAmountWithCharacters()
        {
            Assert.IsFalse(dataCalculator.ValidateAmount("abc"));
        }

        /// <summary>
        /// Testet Validieren eines Betrags mit Zahl.
        /// </summary>
        [TestMethod]
        public void TestValidateAmountWithNumber()
        {
            Assert.IsTrue(dataCalculator.ValidateAmount("10"));
        }

        /// <summary>
        /// Testet Validieren eines Betrags mit Dezimalzahl.
        /// </summary>
        [TestMethod]
        public void TestValidateAmountWithDecimalNumber()
        {
            Assert.IsTrue(dataCalculator.ValidateAmount("10,123"));
        }

        /// <summary>
        /// Testet Validieren eines Betrags mit negativer Zahl.
        /// </summary>
        [TestMethod]
        public void TestValidateAmountWithNegativeNumber()
        {
            Assert.IsTrue(dataCalculator.ValidateAmount("-10"));
            Assert.IsTrue(dataCalculator.ValidateAmount("-10,123"));
        }

        /// <summary>
        /// Testet Konvertieren eines Betrags mit Einnahme.
        /// </summary>
        [TestMethod]
        public void TestConvertAmountWithIncomingAmount()
        {
            Assert.AreEqual("10,00", dataCalculator.ConvertAmount("10", false));
            Assert.AreEqual("5,00", dataCalculator.ConvertAmount("5", false));
        }

        /// <summary>
        /// Testet Konvertieren eines Betrags mit dezimaler Einnahme.
        /// </summary>
        [TestMethod]
        public void TestConvertAmountWithDecimalIncomingAmount()
        {
            Assert.AreEqual("10,12", dataCalculator.ConvertAmount("10,123", false));
            Assert.AreEqual("10,13", dataCalculator.ConvertAmount("10,125", false));
            Assert.AreEqual("10,50", dataCalculator.ConvertAmount("10,5", false));
        }

        /// <summary>
        /// Testet Konvertieren eines Betrags mit negativer Einnahme.
        /// </summary>
        [TestMethod]
        public void TestConvertAmountWithNegativeIncomingAmount()
        {
            Assert.AreEqual("10,00", dataCalculator.ConvertAmount("-10", false));
        }

        /// <summary>
        /// Testet Konvertieren eines Betrags mit Ausgabe.
        /// </summary>
        [TestMethod]
        public void TestConvertAmountWithOutgoingAmount()
        {
            Assert.AreEqual("-10,00", dataCalculator.ConvertAmount("10", true));
        }

        /// <summary>
        /// Testet Konvertieren eines Betrags mit negativer Ausgabe.
        /// </summary>
        [TestMethod]
        public void TestConvertAmountWithNegativeOutgoingAmount()
        {
            Assert.AreEqual("-10,00", dataCalculator.ConvertAmount("-10", true));
        }

        /// <summary>
        /// Testet Berechnen von Saldo.
        /// </summary>
        [TestMethod]
        public void TestCalculateBalance()
        {
            // Arrange
            DateTime dateTime = DateTime.Today;

            Transaction[] transactions =
            {
                new Transaction(dateTime)
                {
                    Amount = "-123,23"
                },
                new Transaction(dateTime)
                {
                    Amount = "-100,10"
                },
                new Transaction(dateTime)
                {
                    Amount = "23,46"
                }
            };

            // Act
            decimal calculatedBalance = dataCalculator.CalculateBalance(transactions);

            // Assert
            Assert.AreEqual(Convert.ToDecimal(-199.87), calculatedBalance);
        }

        /// <summary>
        /// Testet Erstellen von Filter aus Auswahlen.
        /// </summary>
        [TestMethod]
        public void TestCreateFilter()
        {
            // Arrange
            const string EXPECTEDMONTH = "02";

            // Act
            Filter calculatedFilter = dataCalculator.CreateFilter(2, "2014", "Category", "SearchTerm");

            // Assert
            Assert.AreEqual(EXPECTEDMONTH, calculatedFilter.Month);
        }

        /// <summary>
        /// Testet Zurückgeben von Monaten.
        /// </summary>
        [TestMethod]
        public void TestGetMonths()
        {
            Assert.AreEqual(13, dataCalculator.GetMonths().Length);
        }

        /// <summary>
        /// Testet Zurückgeben von Jahren.
        /// </summary>
        [TestMethod]
        public void TestGetYears()
        {
            Assert.AreEqual(3, dataCalculator.GetYears().Length);
        }

        #endregion
    }
}